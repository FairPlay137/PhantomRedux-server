using Microsoft.AspNetCore.Mvc;
using PhantomRedux.Request;
using PhantomRedux.Response;
using PhantomRedux.DebugHelpers;
using System.Text.Json;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;
using System;
using PhantomRedux.Objects;

namespace PhantomRedux.Controllers
{
    [ApiController]
    [Route("intl/top")]
    public class TopController : ControllerBase
    {
        /// <summary>
        /// Lets the game check the online service status. Agnostic response mirrors original server behavior.
        /// </summary>
        [HttpPost]
        [Route("status")]
        [Produces("text/json")]
        public JsonResult Status([FromForm] string post)
        {
            var requestData = JsonSerializer.Deserialize<CommonRequest>(post);
            if (requestData == null) {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }

            DebugHelper.Log($"User ID: {requestData.common.user_id}", 1);
            DebugHelper.Log($"Session ID: {requestData.common.session_id}", 1);
            DebugHelper.Log($"Lang: {requestData.common.lang}", 1);
            DebugHelper.Log($"Platform: {requestData.common.platform}", 1);

            return new JsonResult(new BaseResponse(GameStatusCode.Success, 0, 1)); // assumed that crossover is always true for this action
        }

        [HttpPost]
        [Route("login")]
        [Produces("text/json")]
        public JsonResult Login([FromForm] string post)
        {
            using var conn = Db.Get();

            conn.Open();

            var requestData = JsonSerializer.Deserialize<LoginRequest>(post);
            if (requestData == null)
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }

            DebugHelper.Log($"User ID: {requestData.common.user_id}", 1);
            DebugHelper.Log($"Lang: {requestData.common.lang}", 1);
            DebugHelper.Log($"Platform: {requestData.common.platform}", 1);

            // Retrieve user info matching the provided ID
            var sql = Db.GetCommand(
                @"SELECT * FROM `pha_players` WHERE `id` = '{0}';",
                requestData.common.user_id
            );

            // Execute query
            var command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            // Determine if there's a row to read (there should be 1 if the user exists, or 0 if not)
            if (!reader.HasRows)
            {
                // No user with this ID exists in the database
                return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDoesntExist, 0, 0));
            }

            reader.Read();
            string nickname = reader.GetString("nick_name");
            bool banned = reader.GetBoolean("banned");
            reader.Close();

            if (banned)
            {
                // User is banned from the game
                return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDeleted, 0, 0));
            }

            var sid = "pha_" + GenerateRandomPassword(12);

            var loginTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            var expiryTime = loginTime + Convert.ToInt64(Config.Get("session_time"));

            // Generate SQL to store session
            var insertSessionSql = Db.GetCommand(
                """
                INSERT INTO `pha_sessions` (
                                    sid,
                                    uid,
                                    expiry
                                ) VALUES ('{0}', '{1}', '{2}');
                """,
                sid,
                requestData.common.user_id,
                expiryTime
            );

            string removeStaleSessionsSql = Db.GetCommand(
                @"DELETE FROM `pha_sessions` WHERE expiry < '{0}';",
                loginTime
            );

            // Generate SQL to update player's last login info
            string updatePlayerSql = Db.GetCommand(
                @"UPDATE `pha_players`
                SET
                    last_login = '{0}',
                WHERE `id` = '{1}'",
            loginTime,
                requestData.common.user_id
            );

            command = new MySqlCommand(removeStaleSessionsSql + insertSessionSql + updatePlayerSql, conn);
            command.ExecuteNonQuery();

            return new JsonResult(new LoginResponse(new LoginResponseData()
            {
                user_status = new LoginData_UserStatus(){ session_id = sid },
                bonus = new LoginData_LoginBonus()
                {
                    total_login_day = 0,
                    bonus_kbn = 0
                },
                admin_message = new List<AdminMessage> { new() },
                friend = new LoginData_Friend()
                {
                    mail_num = 0,
                    admin_msg_num = 0,
                    friend_msg_num = 0,
                    friend_num = 0,
                    new_friend_num = 0
                },
                available_ticket = new List<AvailableTicket>(),
                marchenoir = new LoginData_Marchenoir()
                {
                    marche_id = ""
                },
                reward_info = new List<NoahRewardInfo>()
            }));
        }

        private string GenerateRandomPassword(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                // Generate a random number that corresponds to a 
                // valid character, then append that character

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";
                var index = RandomNumberGenerator.GetInt32(chars.Length);
                builder.Append(chars[index]);
            }
            return builder.ToString();
        }
    }
}

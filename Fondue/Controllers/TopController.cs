using Microsoft.AspNetCore.Mvc;
using Fondue.DebugHelpers;
using System.Text.Json;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Text.RegularExpressions;
using Fondue.Objects;
using Fondue.Request;
using Fondue.Response;

namespace Fondue.Controllers
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
            using var conn = Db.Get();

            conn.Open();

            var clientReq = new ClientRequest<CommonRequest>(conn, post, true);

            return new JsonResult(new BaseResponse(GameStatusCode.Success, 0, 1)); // assumed that crossover is always true for this action, as to allow the game to refresh everything properly
        }

        [HttpPost]
        [Route("login")]
        [Produces("text/json")]
        public JsonResult Login([FromForm] string post)
        {
            using var conn = Db.Get();

            conn.Open();

            var clientReq = new ClientRequest<LoginRequest>(conn, post, true);

            // Retrieve user info matching the provided ID
            var sql = Db.GetCommand(@"SELECT * FROM `fon_players` WHERE `user_id` = '{0}';", clientReq.userId);

            // Execute query
            var command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();

            // Determine if there's a row to read (there should be 1 if the user exists, or 0 if not)
            if (!reader.HasRows)
            {
                // No user with this ID exists in the database
                reader.Close();
                return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDoesntExist, 0, 0));
            }

            reader.Read();
            var banned = reader.GetBoolean("banned");
            var suspendedUntil = reader.GetInt64("suspended_until");
            reader.Close();

            if (banned)
            {
                // User is banned from the game
                return new JsonResult(new BaseResponse(GameStatusCode.Err_UserDeleted, 0, 0));
            }

            var loginTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (suspendedUntil > loginTime)
            {
                // User is temporarily suspended
                return new JsonResult(new BaseResponse(GameStatusCode.Err_UserSuspended, 0, 0));
            }

            var sid = "pha_" + GenerateRandomPassword(12);

            var expiryTime = loginTime + Convert.ToInt64(Config.Get("session_time"));

            // Generate SQL to store session
            var insertSessionSql = Db.GetCommand(
                """
                INSERT INTO `fon_sessions` (
                                    sid,
                                    uid,
                                    expiry
                                ) VALUES ('{0}', '{1}', '{2}');
                """,
                sid,
                clientReq.request.common.user_id,
                expiryTime
            );

            var removeStaleSessionsSql = Db.GetCommand(
                @"DELETE FROM `fon_sessions` WHERE expiry < '{0}';",
                loginTime
            );

            // Generate SQL to update player's last login info
            var updatePlayerSql = Db.GetCommand(
                @"UPDATE `fon_players`
                SET
                    last_login = '{0}'
                WHERE `user_id` = '{1}';",
            loginTime,
                clientReq.userId
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

        [HttpPost]
        [Route("info")]
        [Produces("text/json")]
        public JsonResult Info([FromForm] string post)
        {
            using var conn = Db.Get();

            conn.Open();

            var clientReq = new ClientRequest<CommonRequest>(conn, post);
            if (clientReq.error != GameStatusCode.Success) return new JsonResult(new BaseResponse(clientReq.error, 0, 0));

            var response = new InfoResponse();
            var result = response.Populate(conn, clientReq.userId);

            return result == GameStatusCode.Success ? new JsonResult(response) : new JsonResult(new BaseResponse(result, 0, 0));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PhantomRedux.DebugHelpers;
using PhantomRedux.Request;
using PhantomRedux.Response;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PhantomRedux.Controllers
{
    [ApiController]
    [Route("intl/account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("regist")]
        [Produces("text/json")]
        public JsonResult RegisterAccount([FromForm] string post)
        {
            using var conn = Db.Get();

            conn.Open();

            var requestData = JsonSerializer.Deserialize<RegisterRequest>(post);
            if (requestData == null)
            {
                return new JsonResult(new BaseResponse(GameStatusCode.Err_JsonAnalysisFailed, 0, 0));
            }
            string sql;
            MySqlCommand command;

            DebugHelper.Log($"Registering new user with the \"{requestData.account.nick_name}\" nickname...", 1);

            var loginId = GenerateRandomPassword(10);
            var loginTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            var sid = "pha_" + GenerateRandomPassword(12);

            sql = Db.GetCommand(
                @"INSERT INTO `pha_players` (
                        login_id,
                        nick_name,
                        last_login
                    )
                    VALUES ('{0}','{1}','{2}');
                    SELECT LAST_INSERT_ID();",
                loginId,
                requestData.account.nick_name,
                loginTime
            );

            command = new MySqlCommand(sql, conn);

            // Command will return last inserted ID
            var uid = command.ExecuteScalar().ToString();

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
                uid,
                expiryTime
            );

            string removeStaleSessionsSql = Db.GetCommand(
                @"DELETE FROM `pha_sessions` WHERE expiry < '{0}';",
                loginTime
            );

            command = new MySqlCommand(removeStaleSessionsSql+insertSessionSql, conn);
            command.ExecuteNonQuery();

            return new JsonResult(new RegisterResponse(new RegisterResponseData
            {
                user_id = long.Parse(uid),
                login_id = loginId,
                nick_name = requestData.account.nick_name,
                session_id = sid,
                avator_cloth = "",
                avator_hat = "",
                leader_supporter = ""
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

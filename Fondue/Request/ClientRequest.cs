using Microsoft.AspNetCore.Http.HttpResults;
using MySqlConnector;
using System.Text.Json;
using Fondue.DebugHelpers;
using Fondue.Response;

namespace Fondue.Request
{
    public class ClientRequest<T> where T : CommonRequest
    {
        public T request { get; set; }
        public string userId { get; set; }
        public GameStatusCode error { get; set; }

        public ClientRequest(MySqlConnection conn, string post, bool ignore_session = false)
        {
            var deserial = JsonSerializer.Deserialize<T>(post);

            this.request = deserial;
            this.error = GameStatusCode.Success;

            if (!ignore_session)
            {
                if (deserial.common.session_id.Length == 0)
                {
                    DebugHelper.Log("No session ID was given in the request! Use ignore_session!", 3);
                    return;
                }
                var sql = Db.GetCommand("SELECT expiry FROM `fon_sessions` WHERE sid = '{0}';", deserial.common.session_id);
                var command = new MySqlCommand(sql, conn);

                var expiryObj = command.ExecuteScalar();
                if (expiryObj == null)
                {
                    // Session doesn't exist in the table
                    this.error = GameStatusCode.Err_SessionExpired;
                }
                else
                {
                    var expiry = Convert.ToInt64(expiryObj);

                    if (expiry < DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        // Session has expired, remove it from the table
                        sql = Db.GetCommand("DELETE FROM `fon_sessions` WHERE sid = '{0}';",
                            deserial.common.session_id);
                        command = new MySqlCommand(sql, conn);

                        command.ExecuteNonQuery();

                        this.error = GameStatusCode.Err_SessionExpired;
                    }
                    else
                    {
                        sql = Db.GetCommand("SELECT uid FROM `fon_sessions` WHERE sid = '{0}';",
                            deserial.common.session_id);
                        command = new MySqlCommand(sql, conn);

                        var uidObj = command.ExecuteScalar();
                        if (uidObj == null)
                        {
                            this.error = GameStatusCode.Err_UserDoesntExist;
                        }
                        else
                        {
                            this.userId = uidObj.ToString();
                            if (deserial.common.user_id != userId)
                            {
                                DebugHelper.Log(
                                    $"The session ID \"{deserial.common.session_id}\" is NOT associated with the given user ID \"{deserial.common.user_id}\"! It appears to be associated with \"{userId}\" instead! Check session data!",
                                    3);
                            }
                        }
                    }
                }
            }
            else
            {
                this.userId = deserial.common.user_id;
            }
        }
    }
}

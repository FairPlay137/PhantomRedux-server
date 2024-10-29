using Fondue.DebugHelpers;
using Fondue.Objects;
using MySqlConnector;

namespace Fondue.Response
{
    public class InfoResponse : BaseResponse
    {
        public InfoResponseData top { get; set; } = new();

        public GameStatusCode Populate(MySqlConnection conn, string uid)
        {
            var sql = Db.GetCommand(@"SELECT * FROM `fon_players` WHERE user_id = '{0}';", uid);
            var command = new MySqlCommand(sql, conn);

            var reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                // Somehow failed to find player, return error code
                DebugHelper.Log($"Failed to find player with user ID '{uid}'!", 3);
                return GameStatusCode.Err_UserDoesntExist;
            }

            // FIXME: Certain values might have been calculated originally rather than stored within the player database.
            reader.Read();

            var timeTillNextUnit = (int)(reader.GetInt64("next_recovery_unit_at") - DateTimeOffset.Now.ToUnixTimeSeconds());
            if (timeTillNextUnit < 0)
            {
                // no more time until next unit
                timeTillNextUnit = 0;
            }

            top = new InfoResponseData()
            {
                user_data = new InfoData_UserData()
                {
                    login_id = reader.GetString("login_id"),
                    user_id = int.Parse(uid),
                    nick_name = reader.GetString("nick_name")
                },
                user_status = new InfoData_UserStatus()
                {
                    exp = reader.GetInt64("total_exp"),
                    next_exp = reader.GetInt32("next_exp"),
                    current_exp = reader.GetInt32("current_exp"),
                    rank = reader.GetInt32("rank"),
                    stamina = reader.GetInt32("stamina"),
                    wallpaper = reader.GetString("wallpaper"),
                    avator_hat = reader.GetString("avatar_hat"),
                    avator_face = reader.GetString("avatar_face"),
                    avator_cloth = reader.GetString("avatar_clothing"),
                    avator_ot = reader.GetString("avatar_ot"),
                    max_hitpoint = reader.GetInt32("max_hitpoint"),
                    max_stamina = reader.GetInt32("max_stamina"),
                    max_item_num = reader.GetInt32("max_item_num"),
                    max_supporter_num = reader.GetInt32("max_supporter_num"),
                    max_friend_num = reader.GetInt32("max_friend_num"),
                    max_cost = reader.GetInt32("max_cost"),
                    item_available = reader.GetInt32("item_available"),
                    supoprter_available = reader.GetInt32("supporter_available"),
                    recovery_unit_time = reader.GetInt32("recovery_unit_time"),
                    recovery_remain_time = timeTillNextUnit,
                    stamina_recovery_time = reader.GetInt32("stamina_recover_time")
                },
                user_point = new InfoData_UserPoint()
                {
                    medal = reader.GetInt32("medals"),
                    darkmedal = reader.GetInt32("dark_medals"),
                    rcoin = reader.GetInt32("rcoins"),
                    heart = reader.GetInt32("hearts")
                }
            };
            // TODO: populate user_avator and user_wallpaper

            reader.Close();
            return GameStatusCode.Success;
        }
    }

    public class InfoResponseData
    {
        public InfoData_UserData user_data { get; set; }
        public InfoData_UserStatus user_status { get; set; } = new();
        public InfoData_UserPoint user_point { get; set; }
        public List<UserAvatar> user_avator { get; set; } = new();
        public List<UserWallpaper> user_wallpaper { get; set; } = new();
        public List<UserTicket> user_ticket { get; set; } = new();
        public InfoData_SupportPage support_page { get; set; } = new();
    }

    /// <summary>Contains basic information about the user.</summary>
    public class InfoData_UserData
    {
        public string login_id { get; set; }
        public long user_id { get; set; }
        public string nick_name { get; set; }
    }

    /// <summary>Contains various player status data.</summary>
    public class InfoData_UserStatus
    {
        /// <summary>The total amount of experience the user has gained</summary>
        public long exp { get; set; } = 0;

        /// <summary>The amount of EXP required to get to the next rank</summary>
        public int next_exp { get; set; } = 100;

        /// <summary>The amount of EXP required to get to the next rank</summary>
        public int current_exp { get; set; } = 0;

        /// <summary>The current player rank</summary>
        public int rank { get; set; } = 1;

        public int stamina { get; set; } = 20;

        public string wallpaper { get; set; } = "WP000";
        public string avator_hat { get; set; } = "AV200000"; // yes they really did spell "avatar" as "avator"
        public string avator_face { get; set; } = "AV300000";
        public string avator_cloth { get; set; } = "AV100100";
        public string avator_ot { get; set; } = string.Empty;

        public int max_hitpoint { get; set; } = 500;
        public int max_stamina { get; set; } = 20;
        public int max_item_num { get; set; } = 40;
        public int max_supporter_num { get; set; } = 50;
        public int max_friend_num { get; set; } = 30;
        public int max_cost { get; set; } = 40;

        public int item_available { get; set; } = 6;
        public int supoprter_available { get; set; } = 6;

        public int recovery_unit_time { get; set; } = 600;
        public int recovery_remain_time { get; set; } = 0;
        public int stamina_recovery_time { get; set; } = 0;
    }

    public class InfoData_UserPoint
    {
        public int medal { get; set; } = 0;
        public int darkmedal { get; set; } = 0;
        public int rcoin { get; set; } = 0;
        public int heart { get; set; } = 0;
    }

    public class InfoData_SupportPage
    {
        public string url { get; set; } = "https:\\/\\/help.sega.com\\/forums\\/21070565-Rhythm-Thief-Mobile";
    }
}

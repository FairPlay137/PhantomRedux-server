using PhantomRedux.Objects;

namespace PhantomRedux.Response
{
#pragma warning disable IDE1006 // Naming Styles
    public class LoginResponse : BaseResponse
    {
        public LoginResponseData login { get; set; }

        public LoginResponse(LoginResponseData data)
        {
            login = data;
        }
    }

    public class LoginResponseData
    {
        public LoginData_UserStatus user_status { get; set; }
        public LoginData_LoginBonus bonus { get; set; }
        public List<AdminMessage> admin_message { get; set; }
        public LoginData_Friend friend { get; set; }
        public List<AvailableTicket> available_ticket { get; set; }
        public LoginData_Marchenoir marchenoir { get; set; }
        public List<NoahRewardInfo> reward_info { get; set; }
    }

    public class LoginData_UserStatus
    {
        public string session_id { get; set; }
    }

    public class LoginData_LoginBonus
    {
        public int total_login_day { get; set; }
        public int bonus_kbn { get; set; }

        public LoginData_LoginBonus()
        {
            total_login_day = 0;
            bonus_kbn = 0;
        }
    }

    public class LoginData_Friend
    {
        public int mail_num { get; set; }
        public int admin_msg_num { get; set; }
        public int friend_msg_num { get; set; }
        public int friend_num { get; set; }
        public int new_friend_num { get; set; }
    }

    public class LoginData_Marchenoir
    {
        public string marche_id { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}

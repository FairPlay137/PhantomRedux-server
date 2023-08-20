namespace PhantomRedux.Response
{
    public class RegisterResponse : BaseResponse
    {
    }

    public class RegisterResponseData
    {
        public string login_id { get; set; }

        /// <summary>The numeric user ID associated with the account</summary>
        public int user_id { get; set; }

        /// <summary>The alias the player set during registration</summary>
        public string nick_name { get; set; }
        public string leader_supporter { get; set; }
        public string avator_cloth { get; set; }
        public string avator_hat { get; set; }
        public string session_id { get; set; }
    }
}
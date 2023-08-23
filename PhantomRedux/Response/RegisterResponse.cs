namespace PhantomRedux.Response
{
    public class RegisterResponse : BaseResponse
    {
        public RegisterResponseData result { get; set; }
        public RegisterResponse() 
        {
            result = new RegisterResponseData()
            {
                login_id = "DUMMYDUMMY",
                user_id = 12345678,
                nick_name = "Phantom R",
                leader_supporter = "",
                avator_cloth = "",
                avator_hat = "",
                session_id = "DUMMY"
            };
        }

        public RegisterResponse(RegisterResponseData data)
        {
            result = data;
        }
    }

    public class RegisterResponseData
    {
        public string login_id { get; set; }

        /// <summary>The numeric user ID associated with the account</summary>
        public long user_id { get; set; }

        /// <summary>The alias the player set during registration</summary>
        public string nick_name { get; set; }

        public string leader_supporter { get; set; } // doesn't seem to be saved, according to the game code - this is most likely retrieved again in a different endpoint

        public string avator_cloth { get; set; } // doesn't seem to be saved, according to the game code - this is most likely retrieved again in a different endpoint
        public string avator_hat { get; set; } // doesn't seem to be saved, according to the game code - this is most likely retrieved again in a different endpoint

        /// <summary>The assigned session ID for the account</summary>
        public string session_id { get; set; }
    }
}
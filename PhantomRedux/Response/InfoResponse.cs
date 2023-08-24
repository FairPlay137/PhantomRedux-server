namespace PhantomRedux.Response
{
    public class InfoResponse : BaseResponse
    {
    }


    public class InfoData_UserStatus
    {
        /// <summary>The total amount of experience the user has gained</summary>
        public int exp { get; set; }

        /// <summary>The amount of EXP required to get to the next rank</summary>
        public int next_exp { get; set; }

        /// <summary>The amount of EXP required to get to the next rank</summary>
        public int current_exp { get; set; }

        /// <summary>The current player rank</summary>
        public int rank { get; set; }

        public int stamina { get; set; }

        public string wallpaper { get; set; }
        public string avator_hat { get; set; }
        public string avator_face { get; set; }
        public string avator_cloth { get; set; }
        public string avator_ot { get; set; }

        public int max_hitpoint { get; set; }
        public int max_stamina { get; set; }
        public int max_item_num { get; set; }
    }
}

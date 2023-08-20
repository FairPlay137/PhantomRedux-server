namespace PhantomRedux.Response
{
    public class CommonData
    {
        public StatusCode result { get; set; }
        public string app_ver { get; set; }
        public string prot_ver { get; set; }
        public int maintenance { get; set; } // flag
        public long server_time { get; set; }
        public int crossover { get; set; } // flag

        /// <summary>
        /// The constructor for CommonData, which corresponds to the "common" group in the server response.
        /// </summary>
        public CommonData()
        {
            result = StatusCode.Success;
            app_ver = "1.0.0";
            prot_ver = "1";
            maintenance = 0;
            server_time = DateTimeOffset.Now.ToUnixTimeSeconds();
            crossover = 0;
        }

        /// <summary>
        /// The constructor for CommonData, which corresponds to the "common" group in the server response.
        /// </summary>
        /// <param name="rc">The result code</param>
        /// <param name="mFlag">The maintenance flag</param>
        /// <param name="coFlag">The "crossover" flag (indicates that the end of day has been passed and that things should refresh?)</param>
        public CommonData(StatusCode rc, int mFlag, int coFlag)
        {
            result = rc;
            app_ver = "1.0.0";
            prot_ver = "1";
            maintenance = mFlag;
            server_time = DateTimeOffset.Now.ToUnixTimeSeconds();
            crossover = coFlag;
        }
    }
}

namespace PhantomRedux.Request
{
    public class CommonRequest
    {
        public CommonRequestData? common { get; set; }
    }

    public class CommonRequestData
    {
        public string? user_id { get; set; }
        public string? session_id { get; set; }
        public string? lang { get; set; }
        public string? platform { get; set; }
    }
}

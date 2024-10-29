namespace Fondue.Request
{
#pragma warning disable IDE1006 // Naming Styles
    public class LoginRequest : CommonRequest
    {
        public LoginRequestData request { get; set; }
    }

    public class LoginRequestData
    {
        public int login_flg { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}

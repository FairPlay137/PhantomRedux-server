namespace PhantomRedux.Request
{
    public class LoginRequest : CommonRequest
    {
        public LoginRequestData request { get; set; }
    }

    public class LoginRequestData
    {
        public int login_flg { get; set; }
    }
}

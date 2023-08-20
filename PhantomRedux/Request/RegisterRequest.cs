namespace PhantomRedux.Request
{
    public class RegisterRequest : CommonRequest
    {
        public RegisterRequestData? account { get; set; }
    }

    public class RegisterRequestData
    {
        public string nick_name { get; set; }
    }
}

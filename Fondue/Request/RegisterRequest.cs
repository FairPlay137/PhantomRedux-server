namespace Fondue.Request
{
#pragma warning disable IDE1006 // Naming Styles
    public class RegisterRequest : CommonRequest
    {
        public RegisterRequestData? account { get; set; }
    }

    public class RegisterRequestData
    {
        public string nick_name { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}

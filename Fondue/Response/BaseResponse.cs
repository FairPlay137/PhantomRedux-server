namespace Fondue.Response
{
#pragma warning disable IDE1006 // Naming Styles
    public class BaseResponse
    {
        public CommonData common { get; set; }

        public BaseResponse()
        {
            common = new CommonData();
        }
        public BaseResponse(GameStatusCode rc, int mFlag, int coFlag)
        {
            common = new CommonData(rc, mFlag, coFlag);
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}

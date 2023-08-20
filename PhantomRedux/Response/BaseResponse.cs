namespace PhantomRedux.Response
{
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
}

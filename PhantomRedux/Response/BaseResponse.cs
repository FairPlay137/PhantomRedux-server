namespace PhantomRedux.Response
{
    public class BaseResponse
    {
        public CommonData common { get; set; }

        public BaseResponse()
        {
            common = new CommonData();
        }
    }
}

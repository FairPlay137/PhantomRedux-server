namespace PhantomRedux.Response
{
    public class PRExceptionResponse : BaseResponse
    {
        public PRExceptionResponseData exception { get; set; }

        public PRExceptionResponse(Exception e, GameStatusCode rc, int mFlag, int coFlag)
        {
            exception = new PRExceptionResponseData()
            {
                exception_type = e.GetType().Name,
                exception_message = e.Message
            };
            common = new CommonData(rc, mFlag, coFlag);
        }
    }

    public class PRExceptionResponseData
    {
        public string exception_type { get; set; }
        public string exception_message { get; set; }
    }
}

namespace PhantomRedux.Response
{
#pragma warning disable IDE1006 // Naming Styles
    public class MaintenanceResponse : BaseResponse
    {
        public MaintenanceResponseData maintenance { get; set; }

        public MaintenanceResponse()
        {
            maintenance = new MaintenanceResponseData()
            {
                maintenance_title = "Notice",
                maintenance_msg = "The game is currently undergoing maintenance."
            };
            common.maintenance = 1;
        }
    }

    public class MaintenanceResponseData
    {
        public string maintenance_title { get; set; }
        public string maintenance_msg { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}

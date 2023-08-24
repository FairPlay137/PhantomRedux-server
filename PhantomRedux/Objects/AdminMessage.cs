namespace PhantomRedux.Objects
{
#pragma warning disable IDE1006 // Naming Styles
    public class AdminMessage
    {
        public string event_id { get; set; }
        public string title { get; set; }
        public string title_banner { get; set; }
        public string title_icon { get; set; }
        public string body { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public int sort_order { get; set; }

        public AdminMessage()
        {
            event_id = "";
            title = "Welcome to Phantom Redux!";
            title_banner = "";
            title_icon = "";
            body = "{$22} Welcome to Phantom Redux, a replacement server for the iOS version of Rhythm Thief and the Paris Caper!\n"+
                   "{$22} This server is still in development, so not everything will work just yet.\n"+
                   "{$22} DISCLAIMER: This replacement server is not endorsed by or affiliated with SEGA. It is a fan project that " +
                   "is intended to get this version of the game working again after the End of Service on September 30th 2016.";
            text = "";
            icon = "";
            sort_order = 999;
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}

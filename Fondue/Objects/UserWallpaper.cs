namespace Fondue.Objects
{
#pragma warning disable IDE1006 // Naming Styles
    public class UserWallpaper
    {
        /// <summary>The internal ID of the wallpaper.</summary>
        public string wallpaper_id { get; set; }

        /// <summary>Flag for marking the wallpaper as "New!" in the inventory</summary>
        public int new_flg { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}

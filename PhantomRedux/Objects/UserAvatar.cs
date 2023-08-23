namespace PhantomRedux.Objects
{
    public class UserAvatar
    {
        /// <summary>The internal ID of the avatar.</summary>
        public string avator_id { get; set; }

        /// <summary>Flag for marking the avatar as "New!" in the inventory</summary>
        public int new_flg { get; set; }
    }
}

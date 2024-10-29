namespace Fondue.Objects
{
    public class Episode
    {
        /// <summary>The internal ID of the episode</summary>
        public string episode_id { get; set; } = string.Empty;
        public ushort episode_kbn { get; set; } // 2 for event stages, 1 for normal stages
        public string episode_name { get; set; } = string.Empty;
        public ushort guerilla_flg { get; set; }
        public ushort state_kbn { get; set; }
        public ushort buy_flg { get; set; }
        public int rcoin { get; set; }
        public ushort play_flg { get; set; }
        public int mission_total_num { get; set; }
        public List<Mission> mission_list { get; set; }
        public EventData event_data { get; set; }
        public string start_date { get; set; } = string.Empty;
        public string end_date { get; set; } = string.Empty;
    }
}

namespace chatclient.DTOs.Message
{
    public class SearchMessageDTO
    {
        public string messagetext { get; set; }
        public string username { get; set; }
        public string channelid { get; set; }
        public bool hinhanh { get; set; }
        public bool tep { get; set; }
        public string beforeDate { get; set; }
        public string onDate { get; set; }
        public string afterDate { get; set; }

    }
}


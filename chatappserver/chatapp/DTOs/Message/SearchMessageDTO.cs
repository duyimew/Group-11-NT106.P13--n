namespace chatserver.DTOs.Message
{
    public class SearchMessageDTO
    {
        public string messagetext { get; set; } = "";
        public string username { get; set; } = "";
        public string channelid { get; set; } = "";
        public bool hinhanh { get; set; } = false;
        public bool tep { get; set; } = false;
        public string beforeDate { get; set; } = "";
        public string onDate { get; set; } = "";
        public string afterDate { get; set; } = "";

    }
}


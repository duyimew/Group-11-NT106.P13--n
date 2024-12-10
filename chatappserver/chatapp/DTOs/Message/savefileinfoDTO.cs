namespace chatserver.DTOs.Message
{
    public class savefileinfoDTO
    {
        public string UserID { get; set; }
        public string ChannelID { get; set; }
        public string Message { get; set; }
        public string[] filename { get; set; }
    }
}

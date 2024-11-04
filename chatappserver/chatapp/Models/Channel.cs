namespace chatapp.Models
{
    public class Channel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Message> Messages { get; set; }
    }
}

namespace chatapp.Models
{
    public class Channel
    {
        public int ChannelId { get; set; }
        public string? ChannelName { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int? DanhmucId { get; set; }
        public Danhmuc? Danhmuc { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsChat { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}

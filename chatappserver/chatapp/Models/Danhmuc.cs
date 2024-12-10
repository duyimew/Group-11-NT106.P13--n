namespace chatapp.Models
{
    public class Danhmuc
    {
        public int DanhmucId { get; set; }
        public string? DanhmucName { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Channel>? Channels { get; set; }
    }
}

namespace chatapp.Models
{
    public class Channel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }

        // Thuộc về Group (bắt buộc)
        public int GroupId { get; set; }
        public Group Group { get; set; }

        // Có thể thuộc về Danhmuc (tùy chọn)
        public int? DanhmucId { get; set; }
        public Danhmuc Danhmuc { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Xác định kênh này có phải là kênh chat hay không
        public bool IsChat { get; set; }

        // Danh sách tin nhắn thuộc kênh này
        public ICollection<Message> Messages { get; set; }
    }
}

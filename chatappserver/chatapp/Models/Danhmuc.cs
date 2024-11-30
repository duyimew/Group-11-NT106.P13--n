namespace chatapp.Models
{
    public class Danhmuc
    {
        public int DanhmucId { get; set; }
        public string DanhmucName { get; set; }

        // Thuộc về Group (bắt buộc)
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Danh sách các Channel thuộc Danhmuc này
        public ICollection<Channel> Channels { get; set; }
    }
}

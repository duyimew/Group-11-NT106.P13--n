namespace chatapp.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }

        public string Filename { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}

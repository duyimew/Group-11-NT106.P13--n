using System.Net.Mail;

namespace chatapp.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string MessageText { get; set; }
        public bool IsAttachment { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;

        public ICollection<Attachment> Attachments { get; set; }
    }
}

namespace chatapp.Models
{
    public class FriendRequests
    {
        public int SenderId { get; set; }
        public User? Sender { get; set; }
        public int ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;
    }
}

namespace chatapp.Models
{
    public class Friends
    {
        public int UserId_1 { get; set; }
        public User User_1 { get; set; }
        public int UserId_2 { get; set; }
        public User User_2 { get; set; }
        public DateTime FriendedAt { get; set; } = DateTime.Now;
    }
}

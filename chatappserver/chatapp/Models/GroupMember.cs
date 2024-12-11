namespace chatapp.Models
{
    public class GroupMember
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public string? GroupDisplayname { get; set; }
        public string? Role { get; set; }
    }
}

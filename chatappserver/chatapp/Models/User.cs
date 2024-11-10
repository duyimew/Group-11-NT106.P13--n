namespace chatapp.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? UserAva { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<GroupMember> GroupMembers { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

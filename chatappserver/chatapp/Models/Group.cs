using System.Threading.Channels;

namespace chatapp.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<GroupMember> GroupMembers { get; set; }
        public ICollection<Channel> Channels { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

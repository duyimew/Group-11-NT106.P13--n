using System.Threading.Channels;

namespace chatapp.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string? GroupAva { get; set; }
        public string? GroupName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Isprivate { get; set; }

        public string? MaLoiMoi { get; set; }
        //0 là group public
        //1 là group private nhưng là group bắt buộc phải là bạn bè có tối thiều là 1 user tối đa là 10 user
        //2 là group private nhưng là group không bắt buộc phải là bạn bè có tối thiểu và tối đa là 2 user
        public ICollection<GroupMember>? GroupMembers { get; set; }
        public ICollection<Channel>? Channels { get; set; }
        public ICollection<Danhmuc> Danhmucs { get; set; } = new List<Danhmuc>();
    }
}

namespace chatserver.DTOs.GroupMember
{
    public class AddUserDTO
    {
        public string UserID { get; set; }

        public string GroupID { get; set; }
        public string displayname { get; set; }
        public string role { get; set; }

    }
}

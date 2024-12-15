namespace chatserver.DTOs.Friends
{
    public class DeleteFriendRequestDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}

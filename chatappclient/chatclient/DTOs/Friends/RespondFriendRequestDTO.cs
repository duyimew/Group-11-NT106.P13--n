namespace chatclient.DTOs.Friends
{
    public class RespondFriendRequestDTO
    {
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public string action { get; set; }
    }
}
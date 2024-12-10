namespace chatclient.DTOs.Friends
{
    public class RespondFriendRequestDTO
    {
        public string senderUsername { get; set; }
        public string receiverUsername { get; set; }
        public string action { get; set; }
    }
}
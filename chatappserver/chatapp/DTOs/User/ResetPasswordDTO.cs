namespace chatserver.DTOs.User
{
    public class ResetPasswordDTO
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } 
        public string NewPassword { get; set; } 
    }
}

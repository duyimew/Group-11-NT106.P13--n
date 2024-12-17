using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace chatserver.Models
{
    public class OTP
    {

        public int Id { get; set; }


        public string? Code { get; set; } // Mã OTP


        public DateTime CreatedAt { get; set; } // Thời gian tạo OTP


        public DateTime ExpiresAt { get; set; } // Thời gian hết hạn OTP

        public bool IsUsed { get; set; } = false; // Đánh dấu đã sử dụng

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}

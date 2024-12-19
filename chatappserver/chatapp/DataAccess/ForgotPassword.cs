using System.Security.Cryptography;
using System.Net;
using System;
using chatapp.DataAccess;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace chatserver.DataAccess
{
    public class ForgotPassword
    {
        private readonly ConnectDB _connectDB;

        public ForgotPassword(ConnectDB connectDB, Token token)
        {
            _connectDB = connectDB;

        }
        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string HashPassword(string password)
        {
            
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return builder.ToString();
                }
            
        }

        public void SendNewPasswordByEmail(string email, string newPassword)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Chatin'", "chatapp710@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Mật khẩu mới cho ứng dụng Chatin'";

            message.Body = new TextPart("plain")
            {
                Text = $"Mật khẩu mới của bạn là: {newPassword}. Hãy đăng nhập và thay đổi mật khẩu nếu cần."
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("chatapp710@gmail.com", "mlganloukubnhjsd");
                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine("Email đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Chi tiết lỗi: {ex.InnerException.Message}");
                    }
                }
            }
        }


        public void SendOtpByEmail(string email, string otpCode)
    {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Chatin'", "chatapp710@gmail.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "Mã OTP đổi mật khẩu ứng dụng Chatin'";

            message.Body = new TextPart("plain")
            {
                Text = $"Mã OTP của bạn là: {otpCode}. Nó sẽ hết hạn sau 1 phút."
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("chatapp710@gmail.com", "mlganloukubnhjsd");
                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine("Email đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Chi tiết lỗi: {ex.InnerException.Message}");
                    }
                }
            }
        }



}
}

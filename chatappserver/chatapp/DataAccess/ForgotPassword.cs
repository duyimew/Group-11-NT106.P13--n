using System.Net.Mail;
using System.Net;
using System;
using chatapp.DataAccess;

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


        public void SendOtpByEmail(string email, string otpCode)
    {

        string smtpHost = "smtp.gmail.com"; 
        int smtpPort = 587; 
        string smtpUser = "chatapp710@gmail.com"; 
        string smtpPass = "mmymzuivyjhhqmtw"; 


        string subject = "Mã OTP đổi mật khẩu ứng dụng Chatin'";
        string body = $"Mã OTP của bạn là: {otpCode}. Nó sẽ hết hạn sau 1 phút.";

        try
        {

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpUser, "Chatin'");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false; 


                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true; 

                    smtp.Send(mail);
                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi gửi email: {ex.Message}");
        }
    }

}
}

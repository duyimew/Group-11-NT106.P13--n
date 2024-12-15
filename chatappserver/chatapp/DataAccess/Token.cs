using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
namespace chatapp.DataAccess
{
    public class Token
    {
        public async Task<string[]> GenerateToken(string username)
        {
            string[] result = new string[2];
            result[0] = "0";result[1] = "";
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes("your_super_secret_key_which_is_long_enough_32_bytes")); // Đặt một secret key an toàn, tối thiểu 32 bytes
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
        new Claim(ClaimTypes.Name, username),
        new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddDays(1).ToString())
    };

                var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials
                );

                result[1] = new JwtSecurityTokenHandler().WriteToken(token);
                result[0] = "1";
                return result;
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result;
            }
        }
        public bool ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes("your_super_secret_key_which_is_long_enough_32_bytes"));
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string ValidatePassword(string password)
        {
            if (password.Length > 16 || password.Length < 8)
            {
                return "Mật khẩu phải có ít nhất 8 ký tự và tối đa 16 ký tự";
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return "Mật khẩu phải chứa ít nhất một chữ cái viết hoa.";

            if (!Regex.IsMatch(password, @"[a-z]"))
                return "Mật khẩu phải chứa ít nhất một chữ cái viết thường.";

            if (!Regex.IsMatch(password, @"\d"))
                return "Mật khẩu phải chứa ít nhất một chữ số.";

            if (Regex.IsMatch(password, @"[!@#$%^&*(),.?\"":{ }|<>]"))
                return "Mật khẩu không được chứa ký tự đặc biệt (!@#$%^&*(),.?\":{}|<>).";
            return "Hợp lệ";
        }
        public string GenerateRandomPassword(int sokitu)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, sokitu)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void SendEmail(string recipientEmail, string newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("chatapp710@gmail.com");
                mail.To.Add(recipientEmail);
                mail.Subject = "Password Reset Request";
                mail.Body = $"Your new password is: {newPassword}";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("chatapp710@gmail.com", "mmymzuivyjhhqmtw"),
                    EnableSsl = true,
                };

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}

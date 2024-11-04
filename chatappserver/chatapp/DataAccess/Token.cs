using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
        new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(30).ToString())
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
    }
}

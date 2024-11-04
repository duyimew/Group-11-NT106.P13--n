using chatapp.Data;
using Microsoft.Data.SqlClient;

namespace chatapp.DataAccess
{
    public class DangNhap
    {
        private readonly ConnectDB _connectDB;

        public DangNhap(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangNhapUserAsync(string[] userInfo)
        {
            string[] result = new string[1];
            try
            {
                SqlConnection connectionDB = _connectDB.ConnectToDatabase();
                if (connectionDB == null)
                {
                    result[0] = "Kết nối tới database thất bại";
                    return result;
                }
                string login = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                SqlCommand loginCmd = new SqlCommand(login, connectionDB);
                loginCmd.Parameters.AddWithValue("@username", userInfo[1]);
                loginCmd.Parameters.AddWithValue("@password", userInfo[2]);
                int count = (int)loginCmd.ExecuteScalar();
                if (count == 1)
                {
                    result[0] = "1";
                    return result;
                }
                else
                {
                    result[0] = "Tên đăng nhập hoặc mật khẩu sai";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result;
            }
        }
    }
}

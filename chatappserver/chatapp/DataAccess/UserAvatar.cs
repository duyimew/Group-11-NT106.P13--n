
using Microsoft.Data.SqlClient;

namespace chatapp.DataAccess
{
    public class UserAvatar
    {
        private readonly ConnectDB _connectDB;
        public async Task<string[]> CheckUsernameExistAsync(string[] userInfo)
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

                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand(query, connectionDB);
                cmd.Parameters.AddWithValue("@username", userInfo[1]);

                int count = (int)cmd.ExecuteScalar();
                if (count == 1)
                {
                    result[0] = "Username exists"; // Hoặc bạn có thể trả về một thông báo cụ thể khác
                }
                else
                {
                    result[0] = "Username không tồn tại"; // Thông báo nếu không tìm thấy user
                }
                return result;
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result;
            }
        }

    }
}

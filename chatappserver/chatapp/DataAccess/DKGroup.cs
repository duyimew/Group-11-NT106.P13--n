using Microsoft.Data.SqlClient;
using chatapp.Data;

namespace chatapp.DataAccess
{
    public class DKGroup
    {
        private readonly ConnectDB _connectDB;
        public DKGroup(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangkyGroupAsync(string[] userInfo)
        {
            string[] result = new string[1];
            result[0] = "0";
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string checkgroupnameQuery = "SELECT COUNT(*) FROM Groups WHERE GroupName = @groupname";
                    using (SqlCommand checkgroupnameCmd = new SqlCommand(checkgroupnameQuery, connectionDB))
                    {
                        checkgroupnameCmd.Parameters.AddWithValue("@groupname", userInfo[1]);
                        int userExists = (int)await checkgroupnameCmd.ExecuteScalarAsync();
                        if (userExists > 0)
                        {
                            result[0] = "Tên máy chủ đã tồn tại";
                            return result;
                        }
                    }
                    string query = "INSERT INTO Groups (GroupName,CreatedAt) VALUES (@GroupName,@CreatedAt)";
                    SqlCommand command = new SqlCommand(query, connectionDB);
                    command.Parameters.AddWithValue("@GroupName", userInfo[1]);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    command.ExecuteNonQuery();
                    result[0] = "1";
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

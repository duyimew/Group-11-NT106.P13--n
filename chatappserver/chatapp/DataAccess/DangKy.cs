using Microsoft.Data.SqlClient;
using chatapp.Data;

namespace chatapp.DataAccess
{
    public class Dangky
    {
        private readonly ConnectDB _connectDB;
        public Dangky(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangkyUserAsync(string[] userInfo)
        {
            string[] result = new string[2];
            result[0] = "0"; result[1] = "";

            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using (SqlCommand checkUsernameCmd = new SqlCommand(checkUsernameQuery, connectionDB))
                    {
                        checkUsernameCmd.Parameters.AddWithValue("@username", userInfo[1]);
                        int userExists = (int)await checkUsernameCmd.ExecuteScalarAsync();
                        if (userExists > 0)
                        {
                            result[0] = "Tên đăng nhập đã tồn tại";
                            return result;
                        }
                    }

                    string setDateFormatCmdText = "SET DATEFORMAT dmy";
                    using (SqlCommand setDateFormatCmd = new SqlCommand(setDateFormatCmdText, connectionDB))
                    {
                        await setDateFormatCmd.ExecuteNonQueryAsync();
                    }

                    string insertQuery = "INSERT INTO Users (Displayname,Username, Password, Email, FullName, Birthday, CreatedAt) " +
                        "OUTPUT INSERTED.UserId " +
                        "VALUES (@displayname,@username, @password, @Email, @ten, @ngaysinh, @createdAt)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connectionDB))
                    {
                        insertCmd.Parameters.AddWithValue("@displayname", userInfo[1]);
                        insertCmd.Parameters.AddWithValue("@username", userInfo[1]);
                        insertCmd.Parameters.AddWithValue("@password", userInfo[2]);
                        insertCmd.Parameters.AddWithValue("@Email", userInfo[3]);
                        insertCmd.Parameters.AddWithValue("@ten", userInfo[4]);
                        insertCmd.Parameters.AddWithValue("@ngaysinh", userInfo[5]);
                        insertCmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

                        int insertedId = (int)insertCmd.ExecuteScalar();
                        result[1] = insertedId.ToString();
                    }
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

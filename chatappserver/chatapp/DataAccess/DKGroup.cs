using Microsoft.Data.SqlClient;
using chatapp.Data;

namespace chatapp.DataAccess
{
    public class DKGroup
    {
        private readonly ConnectDB _connectDB;
        private readonly Token _token;
        public DKGroup(ConnectDB connectDB,Token token)
        {
            _connectDB = connectDB;
            _token = token;
        }
        public async Task<string[]> DangkyGroupAsync(string[] userInfo)
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
                    string Maloimoi;
                    bool isUnique = false;

                    do
                    {
                        Maloimoi = _token.GenerateRandomPassword(8);

                        // Kiểm tra mã lời mời đã tồn tại hay chưa
                        string checkQuery = "SELECT COUNT(1) FROM Groups WHERE MaLoiMoi = @maloimoi";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connectionDB);
                        checkCommand.Parameters.AddWithValue("@maloimoi", Maloimoi);

                        int count = (int)await checkCommand.ExecuteScalarAsync();
                        isUnique = count == 0; // Mã là duy nhất nếu không có kết quả nào
                    }
                    while (!isUnique);

                    string query = "INSERT INTO Groups (GroupName,CreatedAt,Isprivate,MaLoiMoi) " +
                        "OUTPUT INSERTED.GroupId " +
                        "VALUES (@GroupName,@CreatedAt,@isprivate,@maloimoi)";
                    SqlCommand command = new SqlCommand(query, connectionDB);
                    command.Parameters.AddWithValue("@GroupName", userInfo[1]);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("@isprivate", int.Parse(userInfo[2]));
                    command.Parameters.AddWithValue("@maloimoi", Maloimoi);
                    int insertedId = (int)command.ExecuteScalar();
                    result[0] = "1";
                    result[1] = insertedId.ToString();
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

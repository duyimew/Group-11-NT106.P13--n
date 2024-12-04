using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class DKDanhMuc
    {
        private readonly ConnectDB _connectDB;
        public DKDanhMuc(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangkyDanhmucAsync(string[] userInfo)
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

                    string insertQuery = "INSERT INTO Danhmuc (DanhmucName, GroupId, CreatedAt) " +
                        "OUTPUT INSERTED.DanhmucId " +
                        "VALUES (@DanhmucName, @groupid, @createdat)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connectionDB);
                    insertCmd.Parameters.AddWithValue("@DanhmucName", userInfo[1]);
                    insertCmd.Parameters.AddWithValue("@groupid", userInfo[2]);
                    insertCmd.Parameters.AddWithValue("@createdat", DateTime.Now);

                    int insertedId = (int)insertCmd.ExecuteScalar();
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

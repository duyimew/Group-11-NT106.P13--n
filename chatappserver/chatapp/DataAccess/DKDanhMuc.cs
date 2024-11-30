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
            string[] result = new string[1];
            result[0] = "0";
            string GroupId;

            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }

                    string strQuery = "SELECT GroupId FROM Groups WHERE GroupName = @groupname";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@groupname", userInfo[2]);

                    object groupIdObj = await command.ExecuteScalarAsync();
                    if (groupIdObj == null)
                    {
                        result[0] = "Không tìm thấy tên group";
                        return result;
                    }
                    GroupId = groupIdObj.ToString();

                    // Kiểm tra DanhmucName trong cùng GroupId
                    string checkDanhmucQuery = "SELECT COUNT(*) FROM Danhmuc WHERE DanhmucName = @DanhmucName AND GroupId = @groupid";
                    SqlCommand checkDanhmucCmd = new SqlCommand(checkDanhmucQuery, connectionDB);
                    checkDanhmucCmd.Parameters.AddWithValue("@DanhmucName", userInfo[1]);
                    checkDanhmucCmd.Parameters.AddWithValue("@groupid", GroupId);

                    int danhMucExists = (int)await checkDanhmucCmd.ExecuteScalarAsync();
                    if (danhMucExists > 0)
                    {
                        result[0] = "Tên danh mục đã tồn tại trong nhóm này";
                        return result;
                    }

                    // Thêm Danhmuc mới
                    string insertQuery = "INSERT INTO Danhmuc (DanhmucName, GroupId, CreatedAt) VALUES (@DanhmucName, @groupid, @createdat)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connectionDB);
                    insertCmd.Parameters.AddWithValue("@DanhmucName", userInfo[1]);
                    insertCmd.Parameters.AddWithValue("@groupid", GroupId);
                    insertCmd.Parameters.AddWithValue("@createdat", DateTime.Now);

                    await insertCmd.ExecuteNonQueryAsync();

                    result[0] = "1"; // Thành công
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

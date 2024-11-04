using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class AddUser
    {
        private readonly ConnectDB _connectDB;
        public AddUser(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> AddUserAsync(string[] userInfo)
        {
            string[] result = new string[1];
            string GroupId, UserId;
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string strQuery = "SELECT GroupId, UserId FROM Users,Groups WHERE Username = @username AND GroupName= @groupname";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@username", userInfo[1]);
                    command.Parameters.AddWithValue("@groupname", userInfo[2]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        UserId = row["UserId"].ToString();
                        GroupId = row["GroupId"].ToString();
                    }
                    else
                    {
                        result[0] = "Không tìm thấy username này";
                        return result;
                    }
                    string query = "INSERT INTO GroupMembers(GroupId,UserId) VALUES (@groupid,@userid)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@groupid", GroupId);
                    command1.Parameters.AddWithValue("@userid", UserId);
                    command1.ExecuteNonQuery();
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

using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class FindUser
    {
        private readonly ConnectDB _connectDB;
        private readonly Token _token;
        public FindUser(ConnectDB connectDB, Token token)
        {
            _connectDB = connectDB;
            _token = token;
        }
        public async Task<string[]> FindUserAsync(string[] userInfo)
        {
            List<string> result = new();
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result.Add("Kết nối tới database thất bại");
                        return result.ToArray();
                    }
                    string strQuery = "SELECT Displayname FROM Users WHERE Displayname LIKE @Displayname";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@Displayname", $"%{userInfo[1]}%");
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result.Add("1");
                        foreach (DataRow row in dataTable.Rows)
                        {
                            result.Add(row["Displayname"].ToString());
                        }
                    }
                    else
                    {
                        result.Add("Không tìm thấy user nào");
                    }
                    return result.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.Add($"Error: {ex.Message}");
                return result.ToArray();
            }
        }
        public async Task<string[]> FindUserIDAsync(string[] userInfo)
        {
            List<string> result = new();
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result.Add("Kết nối tới database thất bại");
                        return result.ToArray();
                    }
                    string strQuery = "SELECT UserId FROM Users WHERE Username = @username";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@username", userInfo[1]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result.Add("1");
                        foreach (DataRow row in dataTable.Rows)
                        {
                            result.Add(row["UserId"].ToString());
                        }
                    }
                    else
                    {
                        result.Add("Không tìm thấy user");
                    }
                    return result.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.Add($"Error: {ex.Message}");
                return result.ToArray();
            }
        }
    }
}

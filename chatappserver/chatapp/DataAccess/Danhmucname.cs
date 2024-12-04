using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class Danhmucname
    {
        private readonly ConnectDB _connectDB;
        public Danhmucname(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DanhmucnameAsync(string[] userInfo)
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
                    string strQuery = "SELECT dm.DanhmucId,dm.DanhmucName " +
                        "FROM Danhmuc dm " +
                        "WHERE dm.GroupId=@groupid";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@groupid", userInfo[1]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result = new string[dataTable.Rows.Count + 1];
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            result[i + 1] = dataTable.Rows[i]["DanhmucId"].ToString() + "|" + dataTable.Rows[i]["DanhmucName"].ToString();
                        }
                        result[0] = "1";
                        return result;
                    }
                    else
                    {
                        result[0] = "1";
                        return result;
                    }
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

using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class FindGroup
    {
        private readonly ConnectDB _connectDB;
        public FindGroup(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> FindGroupAsync(string[] userInfo)
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
                    string strQuery = @"SELECT gr.GroupId, gr.GroupName
FROM Groups gr
JOIN GroupMembers gm1 ON gr.GroupId = gm1.GroupId AND gm1.UserId = @userid1
JOIN GroupMembers gm2 ON gr.GroupId = gm2.GroupId AND gm2.UserId = @userid2
WHERE gr.IsPrivate = 2
  AND (SELECT COUNT(*) 
       FROM GroupMembers gm 
       WHERE gm.GroupId = gr.GroupId) = 2;
";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@userid1", int.Parse(userInfo[1]));
                    command.Parameters.AddWithValue("@userid2", int.Parse(userInfo[2]));
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
                            result[i + 1] = dataTable.Rows[i]["GroupId"].ToString()+"|" +dataTable.Rows[i]["GroupName"].ToString();
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

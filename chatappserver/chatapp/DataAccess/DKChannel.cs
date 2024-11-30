using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class DKChannel
    {
        private readonly ConnectDB _connectDB;
        public DKChannel(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangkyChannelAsync(string[] userInfo)
        {
            string[] result = new string[1];
            result[0] = "0";
            string GroupId;
            object danhmucid;
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string checkChannelQuery = "SELECT COUNT(*) FROM Channels WHERE ChannelName = @channelname AND GroupId = (SELECT GroupId FROM Groups WHERE GroupName = @groupname)";
                    SqlCommand checkChannelCommand = new SqlCommand(checkChannelQuery, connectionDB);
                    checkChannelCommand.Parameters.AddWithValue("@channelname", userInfo[1]);
                    checkChannelCommand.Parameters.AddWithValue("@groupname", userInfo[2]);
                    int channelExists = (int)await checkChannelCommand.ExecuteScalarAsync();

                    if (channelExists > 0)
                    {
                        result[0] = "Tên kênh đã tồn tại trong nhóm này";
                        return result;
                    }

                    string strQuery = "SELECT gr.GroupId, dm.DanhmucId FROM Groups gr LEFT JOIN Danhmuc dm ON gr.GroupId = dm.GroupId AND dm.DanhmucName = @danhmucname WHERE gr.GroupName = @groupname;";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@groupname", userInfo[2]);
                    command.Parameters.AddWithValue("@danhmucname",userInfo[4]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        GroupId = row["GroupId"].ToString();
                        danhmucid = row["DanhmucId"] == DBNull.Value ? DBNull.Value : row["DanhmucId"].ToString();
                    }
                    else
                    {
                        result[0] = "Không tìm thấy tên group";
                        return result;
                    }
                    string query = "INSERT INTO Channels(ChannelName,GroupId,DanhmucId,CreatedAt,IsChat) VALUES (@channelname,@groupid,@danhmucid,@createdat,@ischat)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@channelname", userInfo[1]);
                    command1.Parameters.AddWithValue("@groupid", GroupId);
                    command1.Parameters.AddWithValue("@danhmucid", danhmucid);
                    command1.Parameters.AddWithValue("@createdat", DateTime.Now);
                    command1.Parameters.AddWithValue("@ischat", bool.Parse(userInfo[3]));
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

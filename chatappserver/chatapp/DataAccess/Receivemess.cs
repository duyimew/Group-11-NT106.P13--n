using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using chatapp.Models;

namespace chatapp.DataAccess
{
    public class ReceiveMess
    {
        private readonly ConnectDB _connectDB;
        public ReceiveMess(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> ReceiveMessAsync(string[] userInfo)
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
                    string strQuery = "SELECT ms.MessageId, gm.GroupDisplayname, ms.MessageText, STRING_AGG(am.Filename, '; ') AS Filename " +
                        "FROM Messages ms " +
                        "JOIN Channels ch on ch.ChannelId = ms.ChannelId " +
                        "JOIN Groups gr on gr.GroupId = ch.GroupId " +
                        "JOIN GroupMembers gm ON gm.UserId = ms.UserId AND gr.GroupId = gm.GroupId " +
                        "LEFT JOIN Attachments am ON ms.MessageId = am.MessageId " +
                        "WHERE ms.ChannelId = @channelid " +
                        "GROUP BY ms.MessageId, gm.GroupDisplayname, ms.MessageText, ms.SentTime " +
                        "ORDER BY ms.SentTime DESC;";

                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@channelid", userInfo[1]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0 && dataTable.Rows.Count < 100)
                    {
                        result = new string[4 * dataTable.Rows.Count + 1];
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            result[4 * i + 1] = dataTable.Rows[i]["MessageId"].ToString();
                            result[4 * i + 2] = dataTable.Rows[i]["GroupDisplayname"].ToString();
                            result[4 * i + 3] = dataTable.Rows[i]["MessageText"].ToString();
                            result[4 * i + 4] = dataTable.Rows[i]["Filename"].ToString();
                        }
                        result[0] = "1";
                    }
                    else if (dataTable.Rows.Count >= 100)
                    {
                        result = new string[401];
                        for (int i = 0; i < 100; i++)
                        {
                            result[4 * i + 1] = dataTable.Rows[i]["MessageId"].ToString();
                            result[4 * i + 2] = dataTable.Rows[i]["GroupDisplayname"].ToString();
                            result[4 * i + 3] = dataTable.Rows[i]["MessageText"].ToString();
                            result[4 * i + 4] = dataTable.Rows[i]["Filename"].ToString();
                        }
                        result[0] = "1";
                    }
                    else
                    {
                        result[0] = "1";
                        return result;
                    }
                    
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

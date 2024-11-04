using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class SendMess
    {
        private readonly ConnectDB _connectDB;
        public SendMess(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> SendMessAsync(string[] userInfo)
        {
            string[] result = new string[1];
            result[0] = "0";
            string ChannelId, UserId;
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string strQuery = "SELECT ch.ChannelId, us.UserId FROM Users us,Channels ch,Groups gr WHERE us.Username = @username AND ch.ChannelName=@channelname and gr.GroupId=ch.GroupId and gr.GroupName=@groupname";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@username", userInfo[4]);
                    command.Parameters.AddWithValue("@channelname", userInfo[2]);
                    command.Parameters.AddWithValue("@groupname", userInfo[3]);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow row = dataTable.Rows[0];
                        UserId = row["UserId"].ToString();
                        ChannelId = row["ChannelId"].ToString();
                    }
                    else return result;
                    string query = "INSERT INTO Messages(ChannelId,UserId,MessageText,IsAttachment,SentTime) VALUES (@channelid,@userid,@message,@isattachment,@SentTime)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@channelid", ChannelId);
                    command1.Parameters.AddWithValue("@userid", UserId);
                    command1.Parameters.AddWithValue("@message", userInfo[1]);
                    command1.Parameters.AddWithValue("@isattachment", "0");
                    command1.Parameters.AddWithValue("SentTime", DateTime.Now);
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

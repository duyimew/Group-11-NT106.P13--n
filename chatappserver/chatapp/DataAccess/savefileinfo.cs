using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using chatapp.Models;

namespace chatapp.DataAccess
{
    public class savefileinfo
    {
        private readonly ConnectDB _connectDB;
        public savefileinfo(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> savefileinfoAsync(string[] userInfo)
        {
            string[] result = new string[1];
            result[0] = "0";
            string messageid = "", channelid = "", userid = "";
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
                        userid = row["UserId"].ToString();
                        channelid = row["ChannelId"].ToString();
                    }
                    string query = "INSERT INTO Messages(ChannelId,UserId,MessageText,IsAttachment,SentTime) VALUES (@channelid,@userid,@message,@isattachment,@senttime)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@channelid", channelid);
                    command1.Parameters.AddWithValue("@userid", userid);
                    command1.Parameters.AddWithValue("@isattachment", "1");
                    command1.Parameters.AddWithValue("@message", userInfo[1]);
                    command1.Parameters.AddWithValue("@senttime", DateTime.Now);
                    command1.ExecuteNonQuery();
                    string strQuery1 = "select ms.MessageId from Messages ms,Users us,Channels ch,Groups gr where ms.IsAttachment = 1 and ch.ChannelName=@channelname and gr.GroupName=@groupname and us.UserId=ms.UserId and ch.ChannelId=ms.ChannelId and gr.GroupId=ch.GroupId ORDER BY ms.SentTime desc";
                    SqlCommand command3 = new SqlCommand(strQuery1, connectionDB);
                    command3.Parameters.AddWithValue("@channelname", userInfo[2]);
                    command3.Parameters.AddWithValue("@groupname", userInfo[3]);
                    DataTable dataTable1 = new DataTable();
                    using (SqlDataAdapter adapter1 = new SqlDataAdapter(command3))
                    {
                        adapter1.Fill(dataTable1);
                    }
                    if (dataTable1.Rows.Count > 0)
                    {
                        DataRow row1 = dataTable1.Rows[0];
                        messageid = row1["MessageId"].ToString();
                    }
                    for (int i = 5; i < userInfo.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(userInfo[i]))
                        {
                            string query1 = "INSERT INTO Attachments(MessageId, Filename, UploadedAt) VALUES (@messageid, @filename, @uploadedat)";
                            SqlCommand command2 = new SqlCommand(query1, connectionDB);
                            command2.Parameters.AddWithValue("@messageid", messageid);
                            command2.Parameters.AddWithValue("@filename", userInfo[i]);
                            command2.Parameters.AddWithValue("@uploadedat", DateTime.Now);
                            await command2.ExecuteNonQueryAsync();
                        }
                    }
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

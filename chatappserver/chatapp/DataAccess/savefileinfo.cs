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
            string[] result = new string[2+userInfo.Length-4];
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

                    string query = "INSERT INTO Messages(ChannelId,UserId,MessageText,IsAttachment,SentTime) " +
                        "OUTPUT INSERTED.MessageId " +
                        "VALUES (@channelid,@userid,@message,@isattachment,@senttime)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@channelid", userInfo[2]);
                    command1.Parameters.AddWithValue("@userid", userInfo[3]);
                    command1.Parameters.AddWithValue("@isattachment", "1");
                    command1.Parameters.AddWithValue("@message", userInfo[1]);
                    command1.Parameters.AddWithValue("@senttime", DateTime.Now);
                    int messageId = (int)command1.ExecuteScalar();
                    result[1] = messageId.ToString();
                    for (int i = 4; i < userInfo.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(userInfo[i]))
                        {
                            string query1 = "INSERT INTO Attachments(MessageId, Filename, UploadedAt) " +
                                "OUTPUT INSERTED.AttachmentId " +
                                "VALUES (@messageid, @filename, @uploadedat) ";
                            SqlCommand command2 = new SqlCommand(query1, connectionDB);
                            command2.Parameters.AddWithValue("@messageid", messageId.ToString());
                            command2.Parameters.AddWithValue("@filename", userInfo[i]);
                            command2.Parameters.AddWithValue("@uploadedat", DateTime.Now);
                            int AttachmentId = (int)command2.ExecuteScalar();
                            result[i-2] = AttachmentId.ToString();
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

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
            string[] result = new string[2];
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
                    command1.Parameters.AddWithValue("@isattachment", "0");
                    command1.Parameters.AddWithValue("@message", userInfo[1]);
                    command1.Parameters.AddWithValue("@senttime", DateTime.Now);
                    int messageId = (int)command1.ExecuteScalar();
                    result[1] = messageId.ToString();
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

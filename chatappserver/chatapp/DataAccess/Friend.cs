using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using chatapp.DTOs;

namespace chatapp.DataAccess
{
    public class Friend
    {
        private readonly ConnectDB _connectDB;
        public Friend(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> FriendListAsync(int userId)
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
                    string strQuery = "SELECT Username FROM Users WHERE UserId IN (SELECT CASE WHEN UserId_1 = @UserId THEN UserId_2 ELSE UserId_1 END AS FriendId FROM Friends WHERE UserId_1 = @UserId OR UserId_2 = @UserId)";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@UserId", userId);
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
                            result[i + 1] = dataTable.Rows[i]["Username"].ToString();
                        }
                        result[0] = "1";
                        return result;
                    }
                    else return result;
                }
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result;
            }
        }

        public async Task<string[]> ListSentRequest(int userId)
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
                    string strQuery = "SELECT Username FROM Users WHERE UserId IN (SELECT ReceiverId FROM FriendRequests WHERE SenderId = @userId)";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@UserId", userId);
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
                            result[i + 1] = dataTable.Rows[i]["Username"].ToString();
                        }
                        result[0] = "1";
                        return result;
                    }
                    else return result;
                }
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result;
            }
        }
        public async Task<string[]> SendFriendRequest(SendFriendRequestDTO request)
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
                    string strQuery = "SELECT 1 FROM Friends WHERE (UserId_1 = @SenderId AND UserId_2 = @ReceiverId) OR (UserId_2 = @SenderId AND UserId_1 = @ReceiverId)";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderId", request.senderId);
                    command.Parameters.AddWithValue("@ReceiverId", request.receiverId);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result[0] = "Cả hai đã là bạn bè!";
                        return result;
                    }

                    strQuery = "SELECT 1 FROM FriendRequests WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId";
                    command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderId", request.senderId);
                    command.Parameters.AddWithValue("@ReceiverId", request.receiverId);
                    dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result[0] = "Đã gửi yêu cầu kết bạn đi rồi!";
                        return result;
                    }

                    strQuery = "INSERT INTO FriendRequests (SenderId, ReceiverId) VALUES (@SenderId, @ReceiverId)";
                    using (SqlCommand insertCmd = new SqlCommand(strQuery, connectionDB))
                    {
                        insertCmd.Parameters.AddWithValue("@SenderId", request.senderId);
                        insertCmd.Parameters.AddWithValue("@ReceiverId", request.receiverId);

                        await insertCmd.ExecuteNonQueryAsync();
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

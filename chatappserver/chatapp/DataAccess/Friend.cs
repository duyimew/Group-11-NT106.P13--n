using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using chatserver.DTOs.Friends;

namespace chatapp.DataAccess
{
    public class Friend
    {
        private readonly ConnectDB _connectDB;
        public Friend(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> FriendListAsync(string username)
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

                    // Lấy userId từ username
                    string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";
                    SqlCommand userIdCommand = new SqlCommand(userIdQuery, connectionDB);
                    userIdCommand.Parameters.AddWithValue("@Username", username);
                    object userIdObj = await userIdCommand.ExecuteScalarAsync();

                    if (userIdObj == null)
                    {
                        result[0] = "Người dùng không tồn tại";
                        return result;
                    }

                    int userId = Convert.ToInt32(userIdObj);

                    // Lấy danh sách bạn bè
                    string strQuery = @"
                SELECT Username 
                FROM Users 
                WHERE UserId IN 
                (
                    SELECT CASE 
                        WHEN UserId_1 = @UserId THEN UserId_2 
                        ELSE UserId_1 
                    END AS FriendId 
                    FROM Friends 
                    WHERE UserId_1 = @UserId OR UserId_2 = @UserId
                )";
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
                    else
                    {
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

        public async Task<string[]> ListSentRequest(string username)
        {
            string[] result = new string[1];
            result[0] = "0"; // Default to failure status
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }

                    // Get userId from username
                    string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";
                    SqlCommand userIdCommand = new SqlCommand(userIdQuery, connectionDB);
                    userIdCommand.Parameters.AddWithValue("@Username", username);
                    object userIdObj = await userIdCommand.ExecuteScalarAsync();

                    if (userIdObj == null)
                    {
                        result[0] = "Người dùng không tồn tại";
                        return result;
                    }

                    int userId = Convert.ToInt32(userIdObj);

                    // Fetch the list of received friend requests (where the user is the receiver)
                    string strQuery = @"
                SELECT Username 
                FROM Users 
                WHERE UserId IN 
                (
                    SELECT SenderId 
                    FROM FriendRequests 
                    WHERE ReceiverId = @UserId
                )";
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
                        result[0] = "1"; // Success
                        return result;
                    }
                    else
                    {
                        result[0] = "Không có yêu cầu kết bạn nào";
                        return result; // No friend requests
                    }
                }
            }
            catch (Exception ex)
            {
                result[0] = $"Error: {ex.Message}";
                return result; // Error handling
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
                    string strQuery = "WITH UserIds AS (SELECT A.UserId SenderId, B.UserId ReceiverId FROM Users A JOIN Users B ON 1=1 WHERE A.Username = @Sender AND B.Username = @Receiver) SELECT 1 FROM Friends, UserIds WHERE (Friends.UserId_1 = UserIds.SenderId AND Friends.UserId_2 = UserIds.ReceiverId) Or (Friends.UserId_2 = UserIds.SenderId AND Friends.UserId_1 = UserIds.ReceiverId)";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@Sender", request.sender);
                    command.Parameters.AddWithValue("@Receiver", request.receiver);
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

                    strQuery = "WITH UserIds AS (SELECT A.UserId SenderId, B.UserId ReceiverId FROM Users A JOIN Users B ON 1=1 WHERE A.Username = @Sender AND B.Username = @Receiver) SELECT 1 FROM FriendRequests, UserIds WHERE (FriendRequests.SenderId = UserIds.SenderId AND FriendRequests.ReceiverId = UserIds.ReceiverId);";
                    command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@Sender", request.sender);
                    command.Parameters.AddWithValue("@Receiver", request.receiver);
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

                    strQuery = "INSERT INTO FriendRequests (SenderId, ReceiverId, SentTime) " +
                    "SELECT A.UserId AS SenderId, B.UserId AS ReceiverId, GETDATE() " +
                    "FROM Users A JOIN Users B ON 1 = 1 " +
                    "WHERE A.Username = @Sender AND B.Username = @Receiver;";

                    using (SqlCommand insertCmd = new SqlCommand(strQuery, connectionDB))
                    {
                        insertCmd.Parameters.AddWithValue("@Sender", request.sender);
                        insertCmd.Parameters.AddWithValue("@Receiver", request.receiver);

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
        public async Task<string[]> RespondFriendRequest(RespondFriendRequestDTO request)
        {
            string[] result = new string[2];
            result[0] = "0";

            if (request.action != "Accept" && request.action != "Decline")
            {
                result[0] = "Hành động không hợp lệ";
                return result;
            }

            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }

                    // Lấy senderId và receiverId từ username
                    string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";

                    // Lấy senderId
                    SqlCommand senderCmd = new SqlCommand(userIdQuery, connectionDB);
                    senderCmd.Parameters.AddWithValue("@Username", request.senderUsername);
                    object senderIdObj = await senderCmd.ExecuteScalarAsync();

                    if (senderIdObj == null)
                    {
                        result[0] = "Sender không tồn tại";
                        return result;
                    }

                    int senderId = Convert.ToInt32(senderIdObj);

                    // Lấy receiverId
                    SqlCommand receiverCmd = new SqlCommand(userIdQuery, connectionDB);
                    receiverCmd.Parameters.AddWithValue("@Username", request.receiverUsername);
                    object receiverIdObj = await receiverCmd.ExecuteScalarAsync();

                    if (receiverIdObj == null)
                    {
                        result[0] = "Receiver không tồn tại";
                        return result;
                    }

                    int receiverId = Convert.ToInt32(receiverIdObj);

                    // Kiểm tra lời mời kết bạn
                    string strQuery = "SELECT 1 FROM FriendRequests WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderId", senderId);
                    command.Parameters.AddWithValue("@ReceiverId", receiverId);
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count < 1)
                    {
                        result[0] = "Không có lời mời kết bạn hợp lệ";
                        return result;
                    }

                    if (request.action == "Accept")
                    {
                        // Chấp nhận lời mời
                        strQuery = "INSERT INTO Friends (UserId_1, UserId_2) VALUES (@SenderId, @ReceiverId)";
                        using (SqlCommand insertCmd = new SqlCommand(strQuery, connectionDB))
                        {
                            insertCmd.Parameters.AddWithValue("@SenderId", senderId);
                            insertCmd.Parameters.AddWithValue("@ReceiverId", receiverId);
                            await insertCmd.ExecuteNonQueryAsync();
                        }
                        result[1] = "Đã chấp nhận lời mời kết bạn";
                    }
                    else
                    {
                        result[1] = "Đã từ chối lời mời kết bạn";
                    }

                    // Xóa lời mời kết bạn
                    strQuery = "DELETE FROM FriendRequests WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId";
                    using (SqlCommand deleteCmd = new SqlCommand(strQuery, connectionDB))
                    {
                        deleteCmd.Parameters.AddWithValue("@SenderId", senderId);
                        deleteCmd.Parameters.AddWithValue("@ReceiverId", receiverId);
                        await deleteCmd.ExecuteNonQueryAsync();
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

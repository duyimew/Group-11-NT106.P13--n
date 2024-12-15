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
        public async Task<string[]> FriendListAsync(string userid)
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



                    // Lấy danh sách bạn bè
                    string strQuery = @"
                SELECT Displayname 
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
                    command.Parameters.AddWithValue("@UserId", userid);
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
                            result[i + 1] = dataTable.Rows[i]["Displayname"].ToString();
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

        public async Task<string[]> ListSentRequest(string userid)
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


                    // Fetch the list of received friend requests (where the user is the receiver)
                    string strQuery = @"
                SELECT Displayname 
                FROM Users 
                WHERE UserId IN 
                (
                    SELECT SenderId 
                    FROM FriendRequests 
                    WHERE ReceiverId = @UserId
                )";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@UserId", userid);
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
                            result[i + 1] = dataTable.Rows[i]["Displayname"].ToString();
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

                    // Kiểm tra nếu SenderId tồn tại
                    string checkUserQuery = "SELECT 1 FROM Users WHERE UserId = @UserId";
                    SqlCommand command = new SqlCommand(checkUserQuery, connectionDB);
                    command.Parameters.AddWithValue("@UserId", request.senderID);

                    object senderExists = await command.ExecuteScalarAsync();
                    if (senderExists == null)
                    {
                        result[0] = "Người gửi không tồn tại!";
                        return result;
                    }

                    // Kiểm tra nếu ReceiverId tồn tại
                    command = new SqlCommand(checkUserQuery, connectionDB);
                    command.Parameters.AddWithValue("@UserId", request.receiverID);

                    object receiverExists = await command.ExecuteScalarAsync();
                    if (receiverExists == null)
                    {
                        result[0] = "Người nhận không tồn tại!";
                        return result;
                    }

                    // Kiểm tra nếu đã là bạn bè
                    string strQuery = "SELECT 1 FROM Friends WHERE (UserId_1 = @SenderID AND UserId_2 = @ReceiverID) " +
                                      "OR (UserId_2 = @SenderID AND UserId_1 = @ReceiverID)";
                    command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderID", request.senderID);
                    command.Parameters.AddWithValue("@ReceiverID", request.receiverID);

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

                    // Kiểm tra nếu đã gửi yêu cầu kết bạn
                    strQuery = "SELECT 1 FROM FriendRequests WHERE SenderId = @SenderID AND ReceiverId = @ReceiverID";
                    command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderID", request.senderID);
                    command.Parameters.AddWithValue("@ReceiverID", request.receiverID);

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

                    // Gửi yêu cầu kết bạn
                    strQuery = "INSERT INTO FriendRequests (SenderId, ReceiverId, SentTime) " +
                               "VALUES (@SenderID, @ReceiverID, GETDATE())";

                    using (SqlCommand insertCmd = new SqlCommand(strQuery, connectionDB))
                    {
                        insertCmd.Parameters.AddWithValue("@SenderID", request.senderID);
                        insertCmd.Parameters.AddWithValue("@ReceiverID", request.receiverID);

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

                    // Kiểm tra không thể kết bạn với chính mình
                    if (request.senderId == request.receiverId)
                    {
                        result[0] = "Bạn không thể kết bạn với chính mình";
                        return result;
                    }

                    // Kiểm tra lời mời kết bạn
                    string strQuery = "SELECT 1 FROM FriendRequests WHERE SenderId = @SenderId AND ReceiverId = @ReceiverId";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@SenderId", request.senderId);
                    command.Parameters.AddWithValue("@ReceiverId", request.receiverId);
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
                        // Kiểm tra nếu đã là bạn
                        strQuery = "SELECT 1 FROM Friends WHERE (UserId_1 = @SenderId AND UserId_2 = @ReceiverId) OR (UserId_1 = @ReceiverId AND UserId_2 = @SenderId)";
                        SqlCommand checkFriendCmd = new SqlCommand(strQuery, connectionDB);
                        checkFriendCmd.Parameters.AddWithValue("@SenderId", request.senderId);
                        checkFriendCmd.Parameters.AddWithValue("@ReceiverId", request.receiverId);
                        object isAlreadyFriend = await checkFriendCmd.ExecuteScalarAsync();

                        if (isAlreadyFriend != null)
                        {
                            result[0] = "Bạn đã là bạn bè rồi!";
                            return result;
                        }

                        // Chấp nhận lời mời
                        strQuery = "INSERT INTO Friends (UserId_1, UserId_2, FriendedAt) VALUES (@SenderId, @ReceiverId, @FriendedAt)";
                        using (SqlCommand insertCmd = new SqlCommand(strQuery, connectionDB))
                        {
                            insertCmd.Parameters.AddWithValue("@SenderId", request.senderId);
                            insertCmd.Parameters.AddWithValue("@ReceiverId", request.receiverId);
                            insertCmd.Parameters.AddWithValue("@FriendedAt", DateTime.UtcNow); // Thêm thời gian kết bạn
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
                        deleteCmd.Parameters.AddWithValue("@SenderId", request.senderId);
                        deleteCmd.Parameters.AddWithValue("@ReceiverId", request.receiverId);
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

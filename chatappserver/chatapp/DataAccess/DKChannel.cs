﻿using Microsoft.Data.SqlClient;
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
                    string query = "INSERT INTO Channels(ChannelName, GroupId, DanhmucId, CreatedAt, IsChat) " +
                        "OUTPUT INSERTED.ChannelId " +
                        "VALUES (@channelname, @groupid, @danhmucid, @createdat, @ischat) ";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@channelname", userInfo[1]);
                    command1.Parameters.AddWithValue("@groupid", userInfo[2]);
                    command1.Parameters.AddWithValue("@danhmucid", string.IsNullOrEmpty(userInfo[4]) || userInfo[4].ToLower() == "null" ? DBNull.Value : userInfo[4]);
                    command1.Parameters.AddWithValue("@createdat", DateTime.Now);
                    command1.Parameters.AddWithValue("@ischat", bool.Parse(userInfo[3]));
                    int insertedId = (int)command1.ExecuteScalar();
                    result[0] = "1";
                    result[1] = insertedId.ToString();
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

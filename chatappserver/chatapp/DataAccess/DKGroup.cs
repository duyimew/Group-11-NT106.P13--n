﻿using Microsoft.Data.SqlClient;
using chatapp.Data;

namespace chatapp.DataAccess
{
    public class DKGroup
    {
        private readonly ConnectDB _connectDB;
        public DKGroup(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> DangkyGroupAsync(string[] userInfo)
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
                    string query = "INSERT INTO Groups (GroupName,CreatedAt) " +
                        "OUTPUT INSERTED.GroupId " +
                        "VALUES (@GroupName,@CreatedAt)";
                    SqlCommand command = new SqlCommand(query, connectionDB);
                    command.Parameters.AddWithValue("@GroupName", userInfo[1]);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    int insertedId = (int)command.ExecuteScalar();
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

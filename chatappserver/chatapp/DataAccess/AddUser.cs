﻿using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class AddUser
    {
        private readonly ConnectDB _connectDB;
        public AddUser(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> AddUserAsync(string[] userInfo)
        {
            string[] result = new string[1];
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    string query = "INSERT INTO GroupMembers(GroupId,UserId,JoinedAt,GroupDisplayname,Role) VALUES (@groupid,@userid,@joinedAt,@groupDisplayname,@role)";
                    SqlCommand command1 = new SqlCommand(query, connectionDB);
                    command1.Parameters.AddWithValue("@groupid", userInfo[2]);
                    command1.Parameters.AddWithValue("@userid", userInfo[1]);
                    command1.Parameters.AddWithValue("@joinedAt", DateTime.Now);
                    command1.Parameters.AddWithValue("@groupDisplayname", userInfo[3]);
                    command1.Parameters.AddWithValue("@role", userInfo[4]);
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

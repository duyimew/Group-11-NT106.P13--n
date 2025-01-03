﻿using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace chatapp.DataAccess
{
    public class RequestUserRole
    {
        private readonly ConnectDB _connectDB;
        public RequestUserRole(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> RequestUserRoleAsync(string[] userInfo)
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
                    string query = "SELECT UserId,GroupDisplayname,Role " +
                        "FROM GroupMembers " +
                        "Where GroupId =@groupid ";
                    SqlCommand command = new SqlCommand(query, connectionDB);
                    command.Parameters.AddWithValue("@groupid", userInfo[1]);
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
                            result[i + 1] = dataTable.Rows[i]["UserId"].ToString() + "|" + dataTable.Rows[i]["GroupDisplayname"].ToString() + "|" + dataTable.Rows[i]["Role"].ToString();
                        }
                        result[0] = "1";
                        return result;
                    }
                    else
                    {
                        result[0] = "1";
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
    }
}

﻿using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class FindUser
    {
        private readonly ConnectDB _connectDB;
        private readonly Token _token;
        public FindUser(ConnectDB connectDB, Token token)
        {
            _connectDB = connectDB;
            _token = token;
        }
        public async Task<string[]> FindUserAsync(string[] userInfo)
        {
            List<string> result = new();
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result.Add("Kết nối tới database thất bại");
                        return result.ToArray();
                    }
                    string strQuery = "SELECT Username FROM Users WHERE Username LIKE @username";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@username", $"%{userInfo[1]}%");
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    if (dataTable.Rows.Count > 0)
                    {
                        result.Add("1");
                        foreach (DataRow row in dataTable.Rows)
                        {
                            result.Add(row["Username"].ToString());
                        }
                    }
                    else
                    {
                        result.Add("Không tìm thấy user nào");
                    }
                    return result.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.Add($"Error: {ex.Message}");
                return result.ToArray();
            }
        }
    }
}
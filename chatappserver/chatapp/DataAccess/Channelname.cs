﻿using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using chatapp.Models;

namespace chatapp.DataAccess
{
    public class Channelname
    {
        private readonly ConnectDB _connectDB;
        public Channelname(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }
        public async Task<string[]> ChannelNameAsync(string[] userInfo)
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
                    string strQuery = "SELECT ch.ChannelName,ch.IsChat,dm.DanhmucName " +
                        "FROM Channels ch " +
                        "JOIN Groups gr ON gr.GroupId= ch.GroupId " +
                        "LEFT JOIN danhmuc dm on dm.DanhmucId = ch.Danhmucid " +
                        "WHERE gr.GroupName= @groupname ";
                    SqlCommand command = new SqlCommand(strQuery, connectionDB);
                    command.Parameters.AddWithValue("@groupname", userInfo[1]);
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
                            string danhmucname = dataTable.Rows[i]["DanhmucName"] == DBNull.Value ? null : dataTable.Rows[i]["DanhmucName"].ToString();
                            result[i + 1] = dataTable.Rows[i]["ChannelName"].ToString()+"|"+danhmucname+"|"+ dataTable.Rows[i]["IsChat"].ToString();
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
    }
}

using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;
using System.Globalization;

namespace chatapp.DataAccess
{
    public class SearchMess
    {
        private readonly ConnectDB _connectDB;

        public SearchMess(ConnectDB connectDB)
        {
            _connectDB = connectDB;
        }

        public async Task<string[]> SearchMessAsync(string[] userInfo)
        {
            string[] result = new string[1];
            result[0] = "0";

            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }

                    // Câu lệnh SQL an toàn
                    string strQuery = @"
                        SELECT ms.MessageId, gm.GroupDisplayname, ms.MessageText, STRING_AGG(am.Filename, '; ') AS Filename
                        FROM Messages ms
                        JOIN Channels ch on ch.ChannelId = ms.ChannelId
                        JOIN Groups gr on gr.GroupId = ch.GroupId 
                        JOIN GroupMembers gm ON gm.UserId = ms.UserId AND gr.GroupId = gm.GroupId
                        LEFT JOIN Attachments am ON ms.MessageId = am.MessageId
                        WHERE ms.ChannelId = @channelid ";

                    if (!string.IsNullOrEmpty(userInfo[1]))
                    {
                        strQuery += "AND ms.MessageText LIKE @searchText ";
                    }

                    if (!string.IsNullOrEmpty(userInfo[2]))
                    {
                        strQuery += "AND gm.GroupDisplayname = @groupDisplayName ";
                    }

                    if (bool.Parse(userInfo[4]))
                    {
                        strQuery += @"
                            AND (am.Filename LIKE '%.jpg' 
                            OR am.Filename LIKE '%.png' 
                            OR am.Filename LIKE '%.gif' 
                            OR am.Filename LIKE '%.bmp' 
                            OR am.Filename LIKE '%.jpeg')";
                    }

                    if (bool.Parse(userInfo[5]))
                    {
                        strQuery += @"
                            AND (am.Filename LIKE '%.pdf' 
                            OR am.Filename LIKE '%.docx' 
                            OR am.Filename LIKE '%.xlsx' 
                            OR am.Filename LIKE '%.pptx' 
                            OR am.Filename LIKE '%.txt' 
                            OR am.Filename LIKE '%.csv' 
                            OR am.Filename LIKE '%.xml' 
                            OR am.Filename LIKE '%.json' 
                            OR am.Filename LIKE '%.html' 
                            OR am.Filename LIKE '%.css')";
                    }

                    if (!string.IsNullOrEmpty(userInfo[6]))
                    {
                        strQuery += "AND ms.SentTime <= @toDate ";
                    }

                    if (!string.IsNullOrEmpty(userInfo[7]))
                    {
                        strQuery += "AND CAST(ms.SentTime AS DATE) = @onDate ";
                    }

                    if (!string.IsNullOrEmpty(userInfo[8]))
                    {
                        strQuery += "AND ms.SentTime >= @fromDate ";
                    }

                    strQuery += @"
                        GROUP BY ms.MessageId, gm.GroupDisplayname, ms.MessageText, ms.SentTime
                        ORDER BY ms.SentTime DESC;";

                    SqlCommand command = new SqlCommand(strQuery, connectionDB);

                    command.Parameters.AddWithValue("@channelid", int.Parse(userInfo[3]));

                    if (!string.IsNullOrEmpty(userInfo[1]))
                        command.Parameters.AddWithValue("@searchText", $"%{userInfo[1]}%");
                    if (!string.IsNullOrEmpty(userInfo[2]))
                        command.Parameters.AddWithValue("@groupDisplayName", userInfo[2]);

                    DateTime toDate = DateTime.MinValue;
                    if (DateTime.TryParseExact(userInfo[6], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedToDate))
                    {
                        toDate = parsedToDate;
                        command.Parameters.AddWithValue("@toDate", toDate);
                    }

                    DateTime onDate = DateTime.MinValue;
                    if (DateTime.TryParseExact(userInfo[7], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedOnDate))
                    {
                        onDate = parsedOnDate;
                        command.Parameters.AddWithValue("@onDate", onDate);
                    }

                    DateTime fromDate = DateTime.MinValue;
                    if (DateTime.TryParseExact(userInfo[8], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFromDate))
                    {
                        fromDate = parsedFromDate;
                        command.Parameters.AddWithValue("@fromDate", fromDate);
                    }

                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        result = new string[4 * dataTable.Rows.Count + 1];
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            result[4 * i + 1] = dataTable.Rows[i]["MessageId"].ToString();
                            result[4 * i + 2] = dataTable.Rows[i]["GroupDisplayname"].ToString();
                            result[4 * i + 3] = dataTable.Rows[i]["MessageText"].ToString();
                            result[4 * i + 4] = dataTable.Rows[i]["Filename"].ToString();
                        }
                        result[0] = "1";
                    }
                    else
                    {
                        result[0] = "No data found";
                    }

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

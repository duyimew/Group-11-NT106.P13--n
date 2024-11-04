using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class Inforuser
    {
        private readonly ConnectDB _connectDB;
        private readonly Token _token;
        public Inforuser(ConnectDB connectDB,Token token)
        {
            _connectDB = connectDB;
            _token = token;
        }
        public async Task<string[]> InforUserAsync(string[] userInfo)
        {
            string[] result = new string[4];
            result[0] = "0"; result[1] = "";
            result[2] = ""; result[3] = "";
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                        string strQuery = "SELECT Email, FullName, Birthday FROM Users WHERE Username = @username";
                        SqlCommand command = new SqlCommand(strQuery, connectionDB);
                        command.Parameters.AddWithValue("@username", userInfo[1]);
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow row = dataTable.Rows[0];
                            result[1] = row["Email"].ToString();
                            result[2] = row["FullName"].ToString();
                            DateTime birthday = DateTime.Parse(row["Birthday"].ToString());
                            result[3] = birthday.ToString("dd/MM/yyyy");
                            result[0] = "1";
                            return result;
                        }
                        else 
                        {
                            result[0] = "Không tìm thấy username";
                            return result; 
                        };

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

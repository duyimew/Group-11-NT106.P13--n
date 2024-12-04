using Microsoft.Data.SqlClient;
using chatapp.Data;
using System.Data;

namespace chatapp.DataAccess
{
    public class TDDangNhap
    {
        private readonly ConnectDB _connectDB;
        private readonly Token _token;
        public TDDangNhap(ConnectDB connectDB, Token token)
        {
            _connectDB = connectDB;
            _token = token;
        }
        public async Task<string[]> TDDangNhapAsync(string[] userInfo)
        {
            string[] result = new string[3];
            result[0] = ""; result[1] = ""; result[2] = "";
            try
            {
                using (SqlConnection connectionDB = _connectDB.ConnectToDatabase())
                {
                    if (connectionDB == null)
                    {
                        result[0] = "Kết nối tới database thất bại";
                        return result;
                    }
                    if (_token.ValidateToken(userInfo[2]))
                    {
                        string login = "SELECT UserId FROM Users WHERE Username = @username";
                        SqlCommand loginCmd = new SqlCommand(login, connectionDB);
                        loginCmd.Parameters.AddWithValue("@username", userInfo[1]);
                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(loginCmd))
                        {
                            adapter.Fill(dataTable);
                        }
                        if (dataTable.Rows.Count == 1)
                        {
                            result[1] = userInfo[1];
                            result[2] = dataTable.Rows[0]["UserId"].ToString();
                            result[0] = "1";
                            return result;
                        }
                        else
                        {
                            result[0] = "Không tìm thấy user. Vui lòng đăng nhập lại.";
                            return result;
                        }
                    }
                    else
                    {
                        result[0] = "Token không hợp lệ. Vui lòng đăng nhập lại.";
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

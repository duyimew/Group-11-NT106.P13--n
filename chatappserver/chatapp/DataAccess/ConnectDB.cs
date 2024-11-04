using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace chatapp.DataAccess
{
    public class ConnectDB
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration vào constructor của lớp ConnectDB
        public ConnectDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection ConnectToDatabase()
        {
            string connectionString = _configuration.GetConnectionString("ChatAppDatabase");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}

using Microsoft.Data.SqlClient;

namespace BookStoreApi.Database
{
    public class DapperUtility(IConfiguration configuration)
    {
        public SqlConnection GetConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("Default"));
        }
    }
}

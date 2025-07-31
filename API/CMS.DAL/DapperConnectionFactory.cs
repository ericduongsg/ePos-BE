using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Core_App
{
    public class DapperConnectionFactory
    {
        public static IDbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(DAL.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}

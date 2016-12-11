using System.Configuration;

namespace Accounts.Database.DataAccess
{
    public class ConnectionString : IConnectionString
    {
        private readonly string _connectionString;

        public ConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string Get()
        {
            return ConfigurationManager.ConnectionStrings[_connectionString].ToString();
        }
    }
}
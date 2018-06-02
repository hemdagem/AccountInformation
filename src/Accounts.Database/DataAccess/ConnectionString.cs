using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Accounts.Database.DataAccess
{
    public class ConnectionString : IConnectionString
    {
        private readonly IConfiguration _connectionString;

        public ConnectionString(IConfiguration configuration)
        {
            _connectionString = configuration;
        }

        public string Get()
        {
            return _connectionString.GetConnectionString("AccountDb");
        }
    }
}
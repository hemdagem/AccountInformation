using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Accounts.Database.DataAccess.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<IDataReader> ExecuteReader(string storedProcedure, params SqlParameter[] parameters);
        Task<int> ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters);
        T ExecuteScalar<T>(string storedProcedure, params SqlParameter[] parameters);
    }
}

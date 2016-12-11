using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Accounts.Database.DataAccess.Interfaces;

namespace Accounts.Database.DataAccess
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly SqlConnection _sqlconnection;
        private const int _timeout = 30;
        public SqlConnectionFactory(IConnectionString connectionString)
        {
            _sqlconnection = new SqlConnection(connectionString.Get());
        }

        private SqlCommand CreateNewCommand(string storedProcedure, params SqlParameter[] parameters)
        {
            _sqlconnection.Open();
            SqlCommand command = _sqlconnection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedProcedure;
            command.CommandTimeout = _timeout;
            command.Parameters.AddRange(parameters);
            return command;
        }

        public async Task<IDataReader> ExecuteReader(string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = CreateNewCommand(storedProcedure, parameters);
            return  await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public async Task<int> ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            int rowsAffected =0;
            try
            {
                SqlCommand command = CreateNewCommand(storedProcedure, parameters);
                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            catch {  }
            finally
            {
                _sqlconnection.Close();
            }

            return rowsAffected;
        }


        public T ExecuteScalar<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            try
            {
                SqlCommand command = CreateNewCommand(storedProcedure, parameters);
                return (T)Convert.ChangeType(command.ExecuteScalar(),typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlconnection.Close();
            }
        }
    }
}
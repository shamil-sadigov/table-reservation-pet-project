#region

using System.Data;
using Microsoft.Data.SqlClient;

#endregion

namespace Restaurants.Infrastructure
{
    // Will be used by Query part of CQRS
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOrCreateConnection()
        {
            if (_connection is {State: ConnectionState.Open})
                return _connection;

            _connection = new SqlConnection(_connectionString);
            _connection.Open();

            return _connection;
        }

        public void Dispose()
        {
            if (_connection is {State: ConnectionState.Open})
                _connection.Dispose();
        }
    }
}
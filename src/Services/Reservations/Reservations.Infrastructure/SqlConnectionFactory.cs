#region

using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

#endregion

namespace Reservations.Infrastructure
{
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
        
        public ValueTask DisposeAsync()
        {
            return _connection is {State: ConnectionState.Open} 
                ? new ValueTask(Task.Run(_connection.Dispose)) 
                : ValueTask.CompletedTask;
        }
        
        public void Dispose()
        {
            if (_connection is {State: ConnectionState.Open})
                _connection.Dispose();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.Database
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SQLiteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            var conn = new SQLiteConnection(_connectionString);
            await conn.OpenAsync();

            return conn;
        }

        public IDbConnection Create()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            return conn;
        }
    }
}

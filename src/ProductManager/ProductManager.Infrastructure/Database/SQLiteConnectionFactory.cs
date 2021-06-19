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
        public async Task<IDbConnection> CreateAsync()
        {
            throw new NotImplementedException();
        }
    }
}

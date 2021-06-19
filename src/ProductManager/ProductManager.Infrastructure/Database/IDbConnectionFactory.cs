using System.Data;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateAsync();
        IDbConnection Create();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain;

namespace ProductManager.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(string sku);
        Task AddAsync(Product product);
        Task UpdateAsync(Product updatedProduct);
        Task DeleteAsync(string sku);
    }
}

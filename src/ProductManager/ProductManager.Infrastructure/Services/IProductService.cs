using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Infrastructure.DTO;

namespace ProductManager.Infrastructure.Services
{
    public interface IProductService
    {
        Task AddAsync(ProductDto product);
        Task<ProductDto> GetAsync(string sku);
        Task DeleteAsync(string sku);
        Task UpdateWarehouseAsync(WarehouseProductDto warehouseProductDto);
        Task UpdateCatalogAsync(CatalogProductDto catalogProductDto);
        Task UpdateSalesAsync(SalesProductDto salesProductDto);
    }
}

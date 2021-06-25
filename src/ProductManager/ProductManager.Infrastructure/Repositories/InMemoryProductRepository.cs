using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;

namespace ProductManager.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly ISet<Product> _products = new HashSet<Product>();

        public InMemoryProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await Task.FromResult(_products);

        public async Task<Product> GetProductAsync(string sku) =>
            await Task.FromResult(_products.SingleOrDefault(x => x.CatalogProduct.Sku.Sku == sku));

        public Task AddAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(Product updatedProduct)
        {
            var product = await GetProductAsync(updatedProduct.CatalogProduct.Sku.Sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {updatedProduct.CatalogProduct.Sku} was not found!");
            }

            var updatedSales = _mapper.Map(updatedProduct.SalesProduct, product.SalesProduct);
            var newProduct = _mapper.Map<Product, Product>(updatedProduct, product);
        }

        public async Task DeleteAsync(string sku)
        {
            var product = await GetProductAsync(sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {sku} was not found!");
            }

            _products.Remove(product);
        }
    }
}

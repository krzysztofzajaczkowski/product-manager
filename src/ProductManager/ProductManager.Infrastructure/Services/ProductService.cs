using System.Threading.Tasks;
using AutoMapper;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.DTO;

namespace ProductManager.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;

        private readonly IProductRepository _productRepository;
        
        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task AddAsync(ProductDto product)
        {
            var mappedProduct = _mapper.Map<Product>(product);
            if (await _productRepository.GetProductAsync(product.Sku) != null)
            {
                throw new ProductAlreadyExistsException($"Product with sku {product.Sku} already exists!");
            }
            await _productRepository.AddAsync(mappedProduct);
        }

        public async Task<ProductDto> GetAsync(string sku)
        {
            var product = await _productRepository.GetProductAsync(sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {sku} was not found!");
            }
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }

        public async Task DeleteAsync(string sku)
        {
            var product = await _productRepository.GetProductAsync(sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {sku} was not found!");
            }

            await _productRepository.DeleteAsync(sku);
        }

        public async Task UpdateWarehouseAsync(WarehouseProductDto warehouseProductDto)
        {
            var product = await _productRepository.GetProductAsync(warehouseProductDto.Sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {warehouseProductDto.Sku} was not found!");
            }

            _mapper.Map(warehouseProductDto, product);
            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateCatalogAsync(CatalogProductDto catalogProductDto)
        {
            var product = await _productRepository.GetProductAsync(catalogProductDto.Sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {catalogProductDto.Sku} was not found!");
            }

            _mapper.Map(catalogProductDto, product);
            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateSalesAsync(SalesProductDto salesProductDto)
        {
            var product = await _productRepository.GetProductAsync(salesProductDto.Sku);
            if (product == null)
            {
                throw new ProductNotFoundException($"Product with sku {salesProductDto.Sku} was not found!");
            }

            _mapper.Map(salesProductDto, product);
            await _productRepository.UpdateAsync(product);
        }
    }
}
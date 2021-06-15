using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Exceptions;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Services;

namespace ProductManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [Authorize]
        [HttpGet("browse")]
        public async Task<IActionResult> BrowseAsync()
        {
            var products = await _productService.GetAllAsync();

            var dtos = _mapper.Map<List<ProductBlockDto>>(products);

            return Ok(dtos);
        }

        [Authorize]
        [HttpGet("browse/{sku}")]
        public async Task<IActionResult> GetAsync(string sku)
        {
            ProductDto product;
            try
            {
                product = await _productService.GetAsync(sku);
            }
            catch (NotFoundException e)
            {
                // Test Server does not implement CompleteAsync, which is required to allow for 404 response from exception middleware
                return NotFound();
            }

            var dto = _mapper.Map<ProductDto>(product);

            return Ok(dto);
        }

        [Authorize(Roles = "CatalogManager")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<ProductDto>(dto);
            await _productService.AddAsync(product);

            return Created("/products", dto.Sku);
        }

        [Authorize(Roles = "CatalogManager")]
        [HttpPost("update/catalog")]
        public async Task<IActionResult> UpdateCatalogAsync(UpdateCatalogProductDto dto)
        {
            var product = _mapper.Map<CatalogProductDto>(dto);
            try
            {
                await _productService.UpdateCatalogAsync(product);
            }
            catch (NotFoundException e)
            {
                // Test Server does not implement CompleteAsync, which is required to allow for 404 response from exception middleware
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Roles = "WarehouseManager")]
        [HttpPost("update/warehouse")]
        public async Task<IActionResult> UpdateWarehouseAsync(UpdateWarehouseProductDto dto)
        {
            var product = _mapper.Map<WarehouseProductDto>(dto);
            try
            {
                await _productService.UpdateWarehouseAsync(product);
            }
            catch (NotFoundException e)
            {
                // Test Server does not implement CompleteAsync, which is required to allow for 404 response from exception middleware
                return NotFound();
            }

            return Ok();
        }

        [Authorize(Roles = "SalesManager")]
        [HttpPost("update/sales")]
        public async Task<IActionResult> UpdateSalesAsync(UpdateSalesProductDto dto)
        {
            var product = _mapper.Map<SalesProductDto>(dto);
            try
            {
                await _productService.UpdateSalesAsync(product);
            }
            catch (NotFoundException e)
            {
                // Test Server does not implement CompleteAsync, which is required to allow for 404 response from exception middleware
                return NotFound();
            }

            return Ok();
        }
    }
}

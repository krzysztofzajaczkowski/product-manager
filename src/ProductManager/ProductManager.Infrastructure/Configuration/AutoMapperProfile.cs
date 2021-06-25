using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductManager.Core.Domain;
using ProductManager.Core.Domain.ValueObjects;
using ProductManager.Infrastructure.DTO;

namespace ProductManager.Infrastructure.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, Product>();
            CreateMap<WarehouseProduct, WarehouseProduct>();
            CreateMap<SalesProduct, SalesProduct>();
            CreateMap<CatalogProduct, CatalogProduct>();

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CatalogId, x => x.MapFrom(y => y.CatalogProduct.Id))
                .ForMember(x => x.Sku, x => x.MapFrom(y => y.CatalogProduct.Sku.Sku))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.CatalogProduct.Name.Name))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.CatalogProduct.Description.Description))
                .ForMember(x => x.WarehouseId, x => x.MapFrom(y => y.WarehouseProduct.Id))
                .ForMember(x => x.Stock, x => x.MapFrom(y => y.WarehouseProduct.Stock.Stock))
                .ForMember(x => x.Weight, x => x.MapFrom(y => y.WarehouseProduct.Weight.Weight))
                .ForMember(x => x.SalesId, x => x.MapFrom(y => y.SalesProduct.Id))
                .ForMember(x => x.NetPrice, x => x.MapFrom(y => y.SalesProduct.Price.NetPrice))
                .ForMember(x => x.Cost, x => x.MapFrom(y => y.SalesProduct.Price.Cost))
                .ForMember(x => x.TaxPercentage, x => x.MapFrom(y => y.SalesProduct.Price.TaxPercentage));

            CreateMap<ProductDto, Product>()
                .ForMember(x => x.CatalogProduct,
                    x => x.MapFrom(y =>
                        new CatalogProduct(y.CatalogId != Guid.Empty ? y.CatalogId : Guid.NewGuid(), new StockKeepingUnit(y.Sku), new ProductName(y.Name), new ProductDescription(y.Description))))
                .ForMember(x => x.SalesProduct,
                    x => x.MapFrom(y =>
                        new SalesProduct(y.SalesId != Guid.Empty ? y.SalesId : Guid.NewGuid(), new StockKeepingUnit(y.Sku), new Price(y.Cost, y.NetPrice, y.TaxPercentage))))
                .ForMember(x => x.WarehouseProduct,
                    x => x.MapFrom(y =>
                        new WarehouseProduct(y.WarehouseId != Guid.Empty ? y.WarehouseId : Guid.NewGuid(), new StockKeepingUnit(y.Sku), new ProductStock(y.Stock), new ProductWeight(y.Weight))));

            CreateMap<WarehouseProductDto, Product>()
                .ForPath(x => x.WarehouseProduct.Weight, x => x.MapFrom(y => new ProductWeight(y.Weight)))
                .ForPath(x => x.WarehouseProduct.Stock, x => x.MapFrom(y => new ProductStock(y.Stock)));

            CreateMap<CatalogProductDto, Product>()
                .ForPath(x => x.CatalogProduct.Name, x => x.MapFrom(y => new ProductName(y.Name)))
                .ForPath(x => x.CatalogProduct.Description, x => x.MapFrom(y => new ProductDescription(y.Description)));

            CreateMap<SalesProductDto, Product>()
                .ForPath(x => x.SalesProduct.Price,
                    x => x.MapFrom(y => new Price(y.Cost, y.NetPrice, y.TaxPercentage)));
                //.ForPath(x => x.SalesProduct.Price.Cost, x => x.MapFrom(y => y.Cost))
                //.ForPath(x => x.SalesProduct.Price.TaxPercentage, x => x.MapFrom(y => y.TaxPercentage))
                //.ForPath(x => x.SalesProduct.Price.NetPrice, x => x.MapFrom(y => y.NetPrice));

            CreateMap<Product, ProductBlockDto>()
                .ForMember(x => x.Sku, x => x.MapFrom(y => y.CatalogProduct.Sku.Sku))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.CatalogProduct.Name.Name))
                .ForMember(x => x.NetPrice, x => x.MapFrom(y => y.SalesProduct.Price.NetPrice))
                .ForMember(x => x.Stock, x => x.MapFrom(y => y.WarehouseProduct.Stock.Stock));

            CreateMap<ProductDto, ProductBlockDto>();

            CreateMap<CreateProductDto, ProductDto>()
                .ForMember(x => x.Sku, x => x.MapFrom(y => y.Sku))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
                .ForMember(x => x.Cost, x => x.MapFrom(y => 1))
                .ForMember(x => x.NetPrice, x => x.MapFrom(y => 2))
                .ForMember(x => x.TaxPercentage, x => x.MapFrom(y => 0))
                .ForMember(x => x.Stock, x => x.MapFrom(y => 0))
                .ForMember(x => x.Weight, x => x.MapFrom(y => 0));

            CreateMap<UpdateCatalogProductDto, CatalogProductDto>();

            CreateMap<UpdateWarehouseProductDto, WarehouseProductDto>();

            CreateMap<UpdateSalesProductDto, SalesProductDto>();

        }
    }
}

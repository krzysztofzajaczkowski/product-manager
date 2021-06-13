using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductManager.Core.Domain;
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
                .ForMember(x => x.Sku, x => x.MapFrom(y => y.CatalogProduct.Sku))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.CatalogProduct.Name))
                .ForMember(x => x.Description, x => x.MapFrom(y => y.CatalogProduct.Description))
                .ForMember(x => x.WarehouseId, x => x.MapFrom(y => y.WarehouseProduct.Id))
                .ForMember(x => x.Stock, x => x.MapFrom(y => y.WarehouseProduct.Stock))
                .ForMember(x => x.Weight, x => x.MapFrom(y => y.WarehouseProduct.Weight))
                .ForMember(x => x.SalesId, x => x.MapFrom(y => y.SalesProduct.Id))
                .ForMember(x => x.NetPrice, x => x.MapFrom(y => y.SalesProduct.NetPrice))
                .ForMember(x => x.Cost, x => x.MapFrom(y => y.SalesProduct.Cost))
                .ForMember(x => x.TaxPercentage, x => x.MapFrom(y => y.SalesProduct.TaxPercentage));

            CreateMap<ProductDto, Product>()
                .ForMember(x => x.CatalogProduct,
                    x => x.MapFrom(y =>
                        new CatalogProduct(y.CatalogId != Guid.Empty ? y.CatalogId : Guid.NewGuid(),
                            y.Sku, y.Name, y.Description)))
                .ForMember(x => x.SalesProduct,
                    x => x.MapFrom(y =>
                        new SalesProduct(y.SalesId != Guid.Empty ? y.SalesId : Guid.NewGuid(),
                            y.Sku, y.Cost, y.TaxPercentage, y.NetPrice)))
                .ForMember(x => x.WarehouseProduct,
                    x => x.MapFrom(y =>
                        new WarehouseProduct(y.WarehouseId != Guid.Empty ? y.WarehouseId : Guid.NewGuid(), y.Sku, y.Stock, y.Weight)));

            CreateMap<WarehouseProductDto, Product>()
                .ForPath(x => x.WarehouseProduct.Weight, x => x.MapFrom(y => y.Weight))
                .ForPath(x => x.WarehouseProduct.Stock, x => x.MapFrom(y => y.Stock));

            CreateMap<CatalogProductDto, Product>()
                .ForPath(x => x.CatalogProduct.Name, x => x.MapFrom(y => y.Name))
                .ForPath(x => x.CatalogProduct.Description, x => x.MapFrom(y => y.Description));

            CreateMap<SalesProductDto, Product>()
                .ForPath(x => x.SalesProduct.Cost, x => x.MapFrom(y => y.Cost))
                .ForPath(x => x.SalesProduct.TaxPercentage, x => x.MapFrom(y => y.TaxPercentage))
                .ForPath(x => x.SalesProduct.NetPrice, x => x.MapFrom(y => y.NetPrice));
        }
    }
}

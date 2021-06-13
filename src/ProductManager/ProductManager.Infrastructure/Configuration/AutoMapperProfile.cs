using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductManager.Core.Domain;

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
        }
    }
}

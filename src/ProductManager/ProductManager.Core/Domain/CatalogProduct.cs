using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Domain.ValueObjects;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class CatalogProduct : Entity
    {
        public StockKeepingUnit Sku { get; protected set; }
        public ProductName Name { get; protected set; }
        public ProductDescription Description { get; protected set; }

        protected CatalogProduct()
        {
            
        }

        public CatalogProduct(StockKeepingUnit sku, ProductName name, ProductDescription description)
        {
            Sku = sku;
            Name = name;
            Description = description;
        }

        public CatalogProduct(Guid id, StockKeepingUnit sku, ProductName name, ProductDescription description)
        {
            Id = id;
            Sku = sku;
            Name = name;
            Description = description;
        }
    }
}

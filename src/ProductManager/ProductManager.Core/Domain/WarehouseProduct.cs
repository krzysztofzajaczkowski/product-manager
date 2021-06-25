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
    public class WarehouseProduct : Entity
    {
        public StockKeepingUnit Sku { get; protected set; }
        public ProductStock Stock { get; protected set; }
        public ProductWeight Weight { get; protected set; }

        protected WarehouseProduct()
        {
            
        }

        public WarehouseProduct(StockKeepingUnit sku, ProductStock stock, ProductWeight weight)
        {
            Sku = sku;
            Stock = stock;
            Weight = weight;
        }

        public WarehouseProduct(Guid id, StockKeepingUnit sku, ProductStock stock, ProductWeight weight)
        {
            Id = id;
            Sku = sku;
            Stock = stock;
            Weight = weight;
        }

    }
}

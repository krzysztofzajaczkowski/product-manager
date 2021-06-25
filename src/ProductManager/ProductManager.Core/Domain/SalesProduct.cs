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
    public class SalesProduct : Entity
    {
        public StockKeepingUnit Sku { get; protected set; }
        public Price Price { get; protected set; }

        protected SalesProduct()
        {
            
        }

        public SalesProduct(StockKeepingUnit sku, Price price)
        {
            Sku = sku;
            Price = price;
        }

        public SalesProduct(Guid id, StockKeepingUnit sku, Price price)
        {
            Id = id;
            Sku = sku;
            Price = price;
        }

    }
}

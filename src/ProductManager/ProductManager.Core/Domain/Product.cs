using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain.ValueObjects;

namespace ProductManager.Core.Domain
{
    public class Product
    {
        public WarehouseProduct WarehouseProduct { get; protected set; }
        public SalesProduct SalesProduct { get; protected set; }
        public CatalogProduct CatalogProduct { get; protected set; }

        protected Product()
        {
            
        }
        public Product(WarehouseProduct warehouseProduct, SalesProduct salesProduct, CatalogProduct catalogProduct)
        {
            WarehouseProduct = warehouseProduct;
            SalesProduct = salesProduct;
            CatalogProduct = catalogProduct;
        }

        public Product(Guid catalogId, string sku, string name, string description, Guid warehouseId, int stock, double weight,
            Guid salesdId, decimal cost, int taxPercentage, decimal netPrice)
        {
            var stockKeepingUnit = new StockKeepingUnit(sku);
            WarehouseProduct = new WarehouseProduct(warehouseId, stockKeepingUnit, new ProductStock(stock), new ProductWeight(weight));
            SalesProduct = new SalesProduct(salesdId, stockKeepingUnit, new Price(cost, netPrice, taxPercentage));
            CatalogProduct = new CatalogProduct(catalogId, stockKeepingUnit, new ProductName(name), new ProductDescription(description));
        }
        
    }
}

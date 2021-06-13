using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Core.Domain
{
    class Product
    {
        public WarehouseProduct WarehouseProduct { get; }
        public SalesProduct SalesProduct { get; }
        public CatalogProduct CatalogProduct { get; }

        public Product(WarehouseProduct warehouseProduct, SalesProduct salesProduct, CatalogProduct catalogProduct)
        {
            WarehouseProduct = warehouseProduct;
            SalesProduct = salesProduct;
            CatalogProduct = catalogProduct;
        }

        public Product(Guid catalogId, string sku, string name, string description, Guid warehouseId, int stock, double weight,
            Guid salesdId, decimal cost, int taxPercentage, decimal netPrice)
        {
            WarehouseProduct = new WarehouseProduct(warehouseId, sku, stock, weight);
            SalesProduct = new SalesProduct(salesdId, sku, cost, taxPercentage, netPrice);
            CatalogProduct = new CatalogProduct(catalogId, sku, name, description);
        }
        
    }
}

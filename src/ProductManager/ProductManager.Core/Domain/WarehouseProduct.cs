using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class WarehouseProduct : Entity
    {
        private readonly Regex _alphanumericRegex = new Regex(@"^[a-zA-Z0-9]+$");
        public string Sku { get; set; }
        public int Stock { get; set; }
        public double Weight { get; set; }

        public WarehouseProduct(string sku, int stock, double weight)
        {
            Id = Guid.NewGuid();
            SetSku(sku);
            SetStock(stock);
            SetWeight(weight);
        }

        public WarehouseProduct(Guid id, string sku, int stock, double weight)
        {
            Id = id;
            SetSku(sku);
            SetStock(stock);
            SetWeight(weight);
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku) || !_alphanumericRegex.IsMatch(sku))
            {
                throw new InvalidSkuException("Product sku should contain only alphanumeric characters.");
            }
            Sku = sku;
        }

        public void SetStock(int stock)
        {
            if (stock < 0)
            {
                throw new InvalidStockException($"Stock of product with Sku {Sku} can not be less than 0.");
            }

            Stock = stock;
        }

        public void SetWeight(double weight)
        {
            if (weight < 0)
            {
                throw new InvalidWeightException($"Weight of product with Sku {Sku} can not be less than 0.");
            }

            Weight = weight;
        }
    }
}

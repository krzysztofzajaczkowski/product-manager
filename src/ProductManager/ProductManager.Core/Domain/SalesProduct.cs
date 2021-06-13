using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class SalesProduct : Entity
    {
        private readonly Regex _alphanumericRegex = new Regex(@"^[a-zA-Z0-9]+$");
        public string Sku { get; protected set; }
        public decimal Cost { get; protected set; }
        public int TaxPercentage { get; protected set; }
        public decimal NetPrice { get; protected set; }

        public SalesProduct(string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            Id = new Guid();
            SetSku(sku);
            SetCostAndNetPrice(cost, netPrice);
            SetTaxPercentage(taxPercentage);
        }

        public SalesProduct(Guid id, string sku, decimal cost, int taxPercentage, decimal netPrice)
        {
            Id = id;
            SetSku(sku);
            SetCostAndNetPrice(cost, netPrice);
            SetTaxPercentage(taxPercentage);
        }

        public void SetCostAndNetPrice(decimal cost, decimal netPrice)
        {
            if (cost <= 0)
            {
                throw new InvalidCostException($"Cost of product with Sku {Sku} can not be less than or equal to 0.");
            }

            if (netPrice <= 0)
            {
                throw new InvalidNetPriceException($"Net price of product with Sku {Sku} can not be less than or equal to 0.");
            }

            if (netPrice <= cost)
            {
                throw new InvalidNetPriceException($"Net price of product with Sku {Sku} can not be less than or equal to cost.");
            }

            Cost = cost;
            NetPrice = netPrice;
        }

        public void SetNetPrice(decimal netPrice)
        {
            if (netPrice <= 0)
            {
                throw new InvalidNetPriceException($"Net price of product with Sku {Sku} can not be less than or equal to 0.");
            }

            if (netPrice <= Cost)
            {
                throw new InvalidNetPriceException($"Net price of product with Sku {Sku} can not be less than or equal to cost.");
            }

            NetPrice = netPrice;
        }

        public void SetCost(decimal cost)
        {
            if (cost <= 0)
            {
                throw new InvalidCostException($"Cost of product with Sku {Sku} can not be less than or equal to 0.");
            }

            if (cost >= NetPrice)
            {
                throw new InvalidCostException($"Cost of product with Sku {Sku} can not be greater than or equal net price.");
            }

            Cost = cost;
        }

        public void SetTaxPercentage(int taxPercentage)
        {
            if (taxPercentage < 0)
            {
                throw new InvalidTaxPercentageException($"Tax percentage for product with Sku {Sku} can not be less than 0.");
            }

            if (taxPercentage >= 100)
            {
                throw new InvalidTaxPercentageException($"Tax percentage for product with Sku {Sku} can not be greater than or equal to 100.");
            }

            TaxPercentage = taxPercentage;
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku) || !_alphanumericRegex.IsMatch(sku))
            {
                throw new InvalidSkuException("Product sku should contain only alphanumeric characters.");
            }
            Sku = sku;
        }



    }
}

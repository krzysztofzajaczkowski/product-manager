using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class CatalogProduct : Entity
    {
        private readonly Regex _alphanumericRegex = new Regex(@"^[a-zA-Z0-9 ]+$");
        public string Sku { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected CatalogProduct()
        {
            
        }

        public CatalogProduct(string sku, string name, string description)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetSku(sku);
            SetDescription(description);
        }

        public CatalogProduct(Guid id, string sku, string name, string description)
        {
            Id = id;
            SetName(name);
            SetSku(sku);
            SetDescription(description);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_alphanumericRegex.IsMatch(name))
            {
                throw new InvalidProductNameException("Product name should contain only alphanumeric characters and spaces.");
            }
            Name = name;
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku) || !_alphanumericRegex.IsMatch(sku))
            {
                throw new InvalidSkuException("Product sku should contain only alphanumeric characters.");
            }
            Sku = sku;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description) || !_alphanumericRegex.IsMatch(description))
            {
                throw new InvalidDescriptionException("Product description should contain only alphanumeric characters and spaces.");
            }
            Description = description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain.ValueObjects
{
    public class ProductName : ValueObject
    {
        private readonly Regex _nameRegex = new Regex(@"^[a-zA-Z0-9 ]+$");
        public string Name { get; protected set; }

        public ProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_nameRegex.IsMatch(name))
            {
                throw new InvalidProductNameException("Product name should contain only alphanumeric characters and spaces.");
            }
            Name = name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}

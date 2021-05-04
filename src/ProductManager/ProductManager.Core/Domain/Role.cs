using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class Role : Entity
    {
        public string Name { get; protected set; }
        private readonly Regex _nameRegex = new Regex(@"^[a-zA-Z0-9]+$");

        protected Role()
        {
        }

        public Role(Guid id, string name)
        {
            Id = id;
            SetName(name);
        }

        public Role(string name)
        {
            SetName(name);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_nameRegex.IsMatch(name))
            {
                throw new InvalidRoleNameException("Role name should contain only alphanumeric characters.");
            }
            Name = name;
        }
    }
}

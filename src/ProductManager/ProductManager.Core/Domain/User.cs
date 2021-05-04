using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProductManager.Core.Exceptions;

namespace ProductManager.Core.Domain
{
    public class User : Entity
    {
        public string Name { get; protected set; }
        private readonly Regex _nameRegex = new Regex(@"^[a-zA-Z0-9]+$");
        public string Email { get; protected set; }
        private readonly Regex _emailRegex = new Regex(@"^[\w.-]+@(?=[a-z\d][^.]*\.)[a-z\d.-]*[^.]$");
        public string Password { get; protected set; }
        private readonly Regex _passwordRegex = new Regex(@"^[a-zA-Z0-9]");
        protected List<Role> _roles = new List<Role>();
        public IEnumerable<Role> Roles => _roles.AsEnumerable();

        protected User()
        {
        }

        public User(Guid id, string name,
            string email, string password)
        {
            Id = id;
            SetName(name);
            SetEmail(email);
            SetPassword(password);
        }

        public User(string name,
            string email, string password)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetEmail(email);
            SetPassword(password);
        }

        public User(Guid id, string name,
            string email, string password, params Role[] roles)
        {
            Id = id;
            //SetRole(role);
            foreach (var role in roles)
            {
                AddRole(role);
            }
            SetName(name);
            SetEmail(email);
            SetPassword(password);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_nameRegex.IsMatch(name))
            {
                throw new InvalidUsernameException("User name should contain only alphanumeric characters.");
            }
            Name = name;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !_emailRegex.IsMatch(email))
            {
                throw new InvalidEmailException("Email is invalid.");
            }
            Email = email;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || !_passwordRegex.IsMatch(password))
            {
                throw new InvalidPasswordException("User password should contain only alphanumeric characters.");
            }
            Password = password;
        }

        public void AddRole(Role role)
        {
            if (_roles.Any(r => r.Name == role.Name))
            {
                throw new DuplicateRoleException("User already has specified role.");
            }

            _roles.Add(role);
        }

        public void RemoveRole(Role role)
        {
            if (_roles.All(r => r.Name != role.Name))
            {
                throw new RoleNotFoundException("User doesn't have specified role.");
            }

            _roles.Remove(role);
        }
    }

}

using System;
using System.Collections.Generic;
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

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_nameRegex.IsMatch(name))
            {
                throw new InvalidUsernameException("User can not have an empty name.");
            }
            Name = name;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !_emailRegex.IsMatch(email))
            {
                throw new InvalidEmailException("User can not have an empty email.");
            }
            Email = email;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || !_passwordRegex.IsMatch(password))
            {
                throw new InvalidPasswordException("User can not have an empty password.");
            }
            Password = password;
        }
    }

}

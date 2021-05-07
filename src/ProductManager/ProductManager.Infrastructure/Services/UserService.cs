using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Domain;
using ProductManager.Core.Exceptions;
using ProductManager.Core.Repositories;
using ProductManager.Infrastructure.DTO;
using ProductManager.Infrastructure.Extensions;

namespace ProductManager.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
        }

        public async Task<AccountDto> GetAccountAsync(Guid userId)
        {
            var user = await _userRepository.GetOrFailAsync(userId);

            return new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
        }

        public async Task RegisterAsync(Guid userId, string email,
            string name, string password, string roleName = "user")
        {
            var role = await _userRepository.GetRoleOrFailAsync(roleName);

            var user = await _userRepository.GetUserAsync(email);
            if (user != null)
            {
                throw new EmailAlreadyUsedException($"User with email: '{email}' already exists.");
            }
            user = new User(userId, name, email, password, role);
            await _userRepository.AddAsync(user);
        }

        public async Task<JwtDto> LoginAsync(string email, string password, string role)
        {
            var user = await _userRepository.GetUserAsync(email);
            if (user == null)
            {
                throw new InvalidCredentialsException("Invalid credentials.");
            }
            if (user.Password != password)
            {
                throw new InvalidCredentialsException("Invalid credentials.");
            }
            if (user.Roles.All(r => r.Name != role))
            {
                throw new InvalidCredentialsException("Invalid credentials.");
            }

            var jwt = _jwtHandler.CreateToken(user.Id, role);

            return new JwtDto
            {
                Token = jwt.Token,
                Expires = jwt.Expires,
                Role = role
            };
        }
    }
}

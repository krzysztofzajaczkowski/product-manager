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

        public Task<AccountDto> GetAccountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(Guid userId, string email, string name, string password, string roleName = "user")
        {
            throw new NotImplementedException();
        }

        public Task<JwtDto> LoginAsync(string email, string password, string role)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Infrastructure.Services;
using ProductManager.Web.Requests;

namespace ProductManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        protected Guid UserId => User?.Identity?.IsAuthenticated == true ?
            Guid.Parse(User.Identity.Name) :
            Guid.Empty;

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await _userService.GetAccountAsync(id));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _userService.GetAccountAsync(UserId));
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var guid = Guid.NewGuid();
            await _userService.RegisterAsync(guid,
                request.Email, request.Name, request.Password, request.Role);

            return Created("/account", guid);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            return Ok(await _userService.LoginAsync(request.Email, request.Password, request.Role));
        }
    }
}

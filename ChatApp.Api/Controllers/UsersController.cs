// src/ChatApp.Api/Controllers/UsersController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Api.Services;
using ChatApp.Api.DTO;
using System.Threading.Tasks;
using AutoMapper;
using ChatApp.Api.Models;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request);
                if (user == null)
                {
                    return BadRequest("User registration failed.");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
        {
            var token = await _userService.AuthenticateAsync(request);
            if (token == null)
            {
                return Unauthorized(new { error = "Неверный email или пароль" });
            }

            return Ok(new { Token = token });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(sub) ||
                !int.TryParse(sub, out var userId))
            {
                return Unauthorized();
            }

            var users = await _userService.GetAllUsersAsync(userId);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            
            return Ok(usersDto);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Security;
using ChatApp.Api.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Api.Services.Impl;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public UserService(ApplicationDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Email);
        if (existingUser != null)
        {
            throw new ArgumentException("Username already exists");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
        _context.Users?.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<string> AuthenticateAsync(AuthenticateUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    /// <summary>
    /// Выведем всех пользователей, кроме того, что запрашивает метод
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id != userId)
            .ToListAsync();
    }
}
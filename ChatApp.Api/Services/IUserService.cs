using ChatApp.Api.DTO;
using ChatApp.Api.Models;

namespace ChatApp.Api.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequest user);
    Task<string> AuthenticateAsync(AuthenticateUserRequest request);

    Task<IEnumerable<User>> GetAllUsersAsync(int userId);
}
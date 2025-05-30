namespace ChatApp.Api.DTO;

public class AuthenticateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
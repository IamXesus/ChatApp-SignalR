using AutoMapper;
using ChatApp.Api.DTO;
using ChatApp.Api.Models;

namespace ChatApp.Api.Mapping;

public class UserProfile :  Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}
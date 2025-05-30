using AutoMapper;
using ChatApp.Api.DTO;
using ChatApp.Api.Models;

namespace ChatApp.Api.Mapping;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<Chat, ChatDto>()
            .ForMember(dest => dest.Users,
                opt => opt.MapFrom(src => src.Members.Select(m => m.User)));
    }
}
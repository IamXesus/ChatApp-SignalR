using AutoMapper;
using ChatApp.Api.DTO;
using ChatApp.Api.Models;

namespace ChatApp.Api.Mapping;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username));
    }
}
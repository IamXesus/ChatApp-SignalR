using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using ChatApp.Api.DTO;
using ChatApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    public ChatsController(IChatService chatService, IMapper mapper)
    {
        _chatService = chatService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserChats()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(sub) || 
            !int.TryParse(sub, out var userId))
        {
            return Unauthorized();
        }

        var chats = await _chatService.GetUserChatsAsync(userId);
        var chatsDto = _mapper.Map<List<ChatDto>>(chats);
        return Ok(chatsDto);
    }


    [HttpPost("createChat")]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        try
        {
            var chat = await _chatService.CreateChatAsync(request.Name, request.UserIds);
            var chatDto = _mapper.Map<ChatDto>(chat);
            return Ok(chatDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}
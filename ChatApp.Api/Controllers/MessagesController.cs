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
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    public MessagesController(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMessagesByChatIdAsync(int chatId)
    {
        var messages = await _messageService.GetMessagesByChatIdAsync(chatId);
        var  messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
        return Ok(messagesDto);
    }

    [HttpPost("{chatId}")]
    public async Task<IActionResult> SendMessage(int chatId, [FromBody] SendMessageRequest request)
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(sub) || 
            !int.TryParse(sub, out var userId))
        {
            return Unauthorized();
        }        
        try
        {
            var message = await _messageService.SendMessageAsync(chatId, userId, request.Text);
            var messageDto = _mapper.Map<MessageDto>(message);
            return Ok(messageDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{messageId}")]
    public async Task<IActionResult> UpdateMessage(int messageId, [FromBody] UpdateMessageRequest request)
    {
        var success = await _messageService.UpdateMessageAsync(messageId, request.NewText);
        if (!success)
        {
            return NotFound("Message not found.");
        }

        return NoContent();
    }

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        var success = await _messageService.DeleteMessageAsync(messageId);
        if (!success)
        {
            return NotFound("Message not found.");
        }

        return NoContent();
    }
}
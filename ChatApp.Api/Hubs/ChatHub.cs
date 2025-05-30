using System.Security.Claims;
using AutoMapper;
using ChatApp.Api.DTO;
using ChatApp.Api.Models;
using ChatApp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMapper  _mapper;
        public ChatHub(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chatId}");
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat_{chatId}");
        }

        public async Task SendMessage(int chatId, string text)
        {
            try
            {
                var sub = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? throw new HubException("Неавторизованный пользователь");
                if (!int.TryParse(sub, out var userId))
                    throw new HubException("Неверный формат идентификатора пользователя");

                var message = await _messageService.SendMessageAsync(chatId, userId, text);
                
                var messageDto = _mapper.Map<MessageDto>(message);
                await Clients
                    .Group($"chat_{chatId}")
                    .SendAsync("ReceiveMessage", messageDto);
            }
            catch (HubException hex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HubException($"Ошибка при отправке: {ex.Message}");
            }
        }
    }
}
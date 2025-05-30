using ChatApp.Api.Models;

namespace ChatApp.Api.Services;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId);
    Task<Message> SendMessageAsync(int chatId, int userId, string text);
    Task<bool> UpdateMessageAsync(int messageId, string newText);
    Task<bool> DeleteMessageAsync(int messageId);
}
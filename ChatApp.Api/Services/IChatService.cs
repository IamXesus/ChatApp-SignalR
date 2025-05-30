using ChatApp.Api.Models;

namespace ChatApp.Api.Services;

public interface IChatService
{
    Task<IEnumerable<Chat>> GetUserChatsAsync(int userId);
    Task<Chat> CreateChatAsync(string name, List<int> userIds);
}
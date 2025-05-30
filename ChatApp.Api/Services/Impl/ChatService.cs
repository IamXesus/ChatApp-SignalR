using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Impl;

public class ChatService : IChatService
{
    private readonly ApplicationDbContext _context;

    public ChatService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Chat>> GetUserChatsAsync(int userId)
    {
        return await _context.Chats
            .Include(c => c.Members) 
            .ThenInclude(m => m.User) 
            .Where(c => c.Members.Any(m => m.UserId == userId))
            .ToListAsync();
    }

    public async Task<Chat> CreateChatAsync(string name, List<int> userIds)
    {
        //1. Проверим - есть ли у этих пользователей чат с таким названием
        var isDuplicateChatsForUsers =  await _context.Chats
            .Where(c => c.Name == name)
            .Where(c => c.Members.Count == userIds.Count)
            .Where(c => c.Members.All(m => userIds.Contains(m.UserId)))
            .AnyAsync();
        if (isDuplicateChatsForUsers)
        {
            throw new ArgumentException("Chat with the same name already exists.");
        }
        // 2. Создаём чат
        var chat = new Chat { Name = name };
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        // 3. Формируем связи ChatUser
        var members = userIds
            .Select(uid => new ChatUser {
                ChatId   = chat.Id,
                UserId   = uid
            })
            .ToList();

        _context.Set<ChatUser>().AddRange(members);
        await _context.SaveChangesAsync();
        
        await _context.Entry(chat)
            .Collection(c => c.Members)
            .Query()
            .Include(m => m.User)
            .LoadAsync();
        
        return chat;
    }
}
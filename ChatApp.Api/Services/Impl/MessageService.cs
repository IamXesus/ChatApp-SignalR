using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Impl;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;

    public MessageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
    {
        return await _context.Messages
            .AsNoTracking()
            .Include(m => m.User)
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message> SendMessageAsync(int chatId, int userId, string text)
    {
        var chat = await _context.Chats.Include(c => c.Members).FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat == null)
        {
            throw new ArgumentException("Chat does not exist.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException("User does not exist.");
        }

        if (!chat.Members.Any(u => u.UserId == userId))
        {
            throw new ArgumentException("User is not part of this chat.");
        }

        var message = new Message
        {
            Text = text,
            UserId = userId,
            ChatId = chatId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<bool> UpdateMessageAsync(int messageId, string newText)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
        {
            return false;
        }

        message.Text = newText;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteMessageAsync(int messageId)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message == null)
        {
            return false;
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return true;
    }
}
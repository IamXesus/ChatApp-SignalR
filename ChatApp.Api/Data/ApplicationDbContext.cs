using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    
    public DbSet<ChatUser> ChatUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // составной ключ
        modelBuilder.Entity<ChatUser>()
            .HasKey(cu => new { cu.ChatId, cu.UserId });

        // навигации
        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.Chat)
            .WithMany(c => c.Members)
            .HasForeignKey(cu => cu.ChatId);

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ChatLinks)
            .HasForeignKey(cu => cu.UserId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(m => m.Messages)
            .HasForeignKey(f => f.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
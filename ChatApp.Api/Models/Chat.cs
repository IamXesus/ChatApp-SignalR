namespace ChatApp.Api.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<ChatUser> Members { get; set; } = new List<ChatUser>();
    public ICollection<Message>  Messages { get; set; } = new List<Message>();
}
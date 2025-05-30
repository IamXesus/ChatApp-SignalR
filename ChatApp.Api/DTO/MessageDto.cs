namespace ChatApp.Api.DTO;

// DTO
public class MessageDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public string   Username  { get; set; }

}
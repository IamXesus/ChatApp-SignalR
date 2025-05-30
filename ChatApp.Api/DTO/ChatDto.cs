namespace ChatApp.Api.DTO;

public class ChatDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserDto> Users { get; set; }
}
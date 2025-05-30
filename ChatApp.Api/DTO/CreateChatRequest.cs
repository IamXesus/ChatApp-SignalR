namespace ChatApp.Api.DTO;

public class CreateChatRequest
{
    public string Name { get; set; }
    public List<int> UserIds { get; set; }
}
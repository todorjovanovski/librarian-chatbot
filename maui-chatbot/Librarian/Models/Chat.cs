namespace Librarian.Models;

public class Chat
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Message> Messages = [];
}
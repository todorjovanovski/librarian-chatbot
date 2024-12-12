namespace Librarian.Models;

public class Message
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Sender Sender { get; set; }
    public string Content { get; set; } = string.Empty;
}
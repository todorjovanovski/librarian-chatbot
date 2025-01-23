using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Librarian.Models;

public class Message
{
    [PrimaryKey] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(typeof(Chat))]
    public Guid ChatId { get; set; }
    
    public DateTime Time { get; set; } = DateTime.UtcNow;
    
    public Sender Sender { get; set; }

    public string Content { get; set; } = string.Empty;
}
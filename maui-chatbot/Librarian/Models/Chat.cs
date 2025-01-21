using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Librarian.Models;

public class Chat
{
    [PrimaryKey]
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Message> Messages { get; set; } = [];
}
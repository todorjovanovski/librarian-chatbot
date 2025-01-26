using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Librarian.Models;

public class Chat
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Title { get; set; } = string.Empty;
    
    [OneToMany(CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
    public List<Message> Messages { get; set; } = [];
}
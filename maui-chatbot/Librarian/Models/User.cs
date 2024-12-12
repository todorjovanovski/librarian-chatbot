namespace Librarian.Models;

public class User
{
    public Guid Id { get; set; }
    public List<Chat> ChatRooms { get; set; } = [];
}
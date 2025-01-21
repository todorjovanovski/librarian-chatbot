using Librarian.Models;

namespace Librarian.Repository;

public interface IRepository
{
    Task InitializeDatabase();
    
    Task<Message> GetMessage(Guid id);
    Task<List<Message>> GetAllMessages(Guid chatId);
    Task InsertMessage(Message message);

    Task<Chat> GetChat(Guid id);
    Task<List<Chat>> GetAllChats();
    Task InsertChat(Chat chat);
    Task<bool> DeleteChat(Chat chat);
}
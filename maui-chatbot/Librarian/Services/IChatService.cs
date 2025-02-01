using Librarian.Models;

namespace Librarian.Services;

public interface IChatService
{
    Task<Chat> StartNewChat(string title);
    Task<Chat> GetCurrentChat(Guid id);
    Task AddMessage(Message message);
    Task<List<Chat>> GetAllChats();
    Task DeleteChat(Guid chatId);
}
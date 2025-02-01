using Librarian.Models;
using Librarian.Repository;

namespace Librarian.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IRepository _repository;

    public ChatService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Chat> StartNewChat(string title)
    {
        var chat = new Chat { Title = title };
        await _repository.InsertChat(chat);
        return chat;
    }

    public async Task<Chat> GetCurrentChat(Guid id)
    {
        return await _repository.GetChat(id);
    }

    public async Task AddMessage(Message message)
    {
        await _repository.InsertMessage(message);
    }

    public async Task<List<Chat>> GetAllChats()
    {
        return await _repository.GetAllChats();
    }

    public async Task DeleteChat(Guid chatId)
    {
        var chat = await GetCurrentChat(chatId);
        await _repository.DeleteChat(chat);
    }
}
using Librarian.Constants;
using Librarian.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace Librarian.Repository.Implementations;

public class Repository : IRepository
{
    private readonly SQLiteAsyncConnection _databaseConnection;

    public Repository()
    {
        _databaseConnection = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
    }

    public async Task InitializeDatabase()
    {
        await _databaseConnection.CreateTableAsync<Chat>();
        await _databaseConnection.CreateTableAsync<Message>();
    }

    public async Task<Message> GetMessage(Guid id)
    {
        return await _databaseConnection.GetWithChildrenAsync<Message>(id);
    }

    public async Task<List<Message>> GetAllMessages(Guid chatId)
    {
        return await _databaseConnection.GetAllWithChildrenAsync<Message>(message => message.ChatId == chatId);
    }

    public async Task InsertMessage(Message message)
    {
        await _databaseConnection.InsertAsync(message);
    }

    public async Task<Chat> GetChat(Guid id)
    {
        return await _databaseConnection.GetWithChildrenAsync<Chat>(id, true);
    }

    public async Task<List<Chat>> GetAllChats()
    {
        return await _databaseConnection.GetAllWithChildrenAsync<Chat>();
    }

    public async Task InsertChat(Chat chat)
    {
        await _databaseConnection.InsertAsync(chat);
    }

    public async Task<bool> DeleteChat(Chat chat)
    {
        return await _databaseConnection.DeleteAsync(chat) > 0;
    }
}
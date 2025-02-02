using System.Net.Http.Headers;
using System.Text.Json;
using Librarian.Constants;
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

    public async Task<Chat> StartNewChat(Stream pdfContent, string title)
    {
        using var client = new HttpClient();
        using var form = new MultipartFormDataContent();
        
        var fileContent = new StreamContent(pdfContent);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        form.Add(fileContent, "book", $"{title}.pdf");
        form.Add(new StringContent(title), "title");
        
        try
        {
            using var response = await client.PostAsync(EndpointConstants.UploadPdf, form);
            response.EnsureSuccessStatusCode();
            var chat = JsonSerializer.Deserialize<Chat>(await response.Content.ReadAsStringAsync())!;
            await _repository.InsertChat(chat);
            return chat;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
        using var client = new HttpClient();

        try
        {
            var response = await client.DeleteAsync(EndpointConstants.DeleteChat(chatId));
            response.EnsureSuccessStatusCode();
            await _repository.DeleteChat(chat);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task<Message> AskQuestion(Message question, string title, Guid chatId)
    {
        await _repository.InsertMessage(question);
        using var client = new HttpClient();
        var formData = new Dictionary<string, string>
        {
            { "question", question.Content },
            { "title", title },
            { "book_id", chatId.ToString() }
        };

        var content = new FormUrlEncodedContent(formData);

        try
        {
            var response = await client.PostAsync(EndpointConstants.AskQuestion, content);
            response.EnsureSuccessStatusCode();
            var interaction =JsonSerializer.Deserialize<Interaction>(await response.Content.ReadAsStringAsync());
            var botResponse = new Message
            {
                ChatId = chatId,
                Content = interaction!.Answer.Split("Answer:")[1].Trim(),
                Sender = Sender.ChatBot
            };
            await _repository.InsertMessage(botResponse);
            return botResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
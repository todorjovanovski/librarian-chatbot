namespace Librarian.Constants;

public static class EndpointConstants
{
    public const string BaseUrl = "http://<YOUR_CURRENT_IP_ADDRESS>:8000";
    public const string UploadPdf = $"{BaseUrl}/upload_book";
    public const string AskQuestion = $"{BaseUrl}/ask";
    public static string DeleteChat(Guid chatId) => $"{BaseUrl}/delete_chat/{chatId}";
}
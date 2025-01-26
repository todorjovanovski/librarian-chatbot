using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.Pages;
using Librarian.Services;
using Librarian.ViewModels.Base;
namespace Librarian.ViewModels;

public partial class UploadViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IChatService _chatService;

    public string PdfName
        => PdfFile is null ? string.Empty : PdfFile.FileName;

    public ImageSource UploadPdfIcon
        => PdfFile is null ? ImageSource.FromFile("attachment_icon") : ImageSource.FromFile("new_chat_icon");

    public string UploadPdfLabel
        => PdfFile is null ? "Upload Pdf" : "Start new chat";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PdfName), nameof(UploadPdfIcon), nameof(UploadPdfLabel))]
    private FileResult? _pdfFile;

    [ObservableProperty]
    private string _chatTitle = string.Empty;

    [ObservableProperty]
    private bool _errorMessage = false;

    public UploadViewModel(INavigationService navigationService, IChatService chatService)
    {
        _navigationService = navigationService;
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task ChooseFile()
    {
        ErrorMessage = false;
        if (PdfFile is null)
        {
            try
            {
                var options = new PickOptions
                {
                    PickerTitle = "Choose a PDF file to upload.",
                    FileTypes = FilePickerFileType.Pdf
                };

                PdfFile = await MainThread.InvokeOnMainThreadAsync(() => FilePicker.Default.PickAsync(options));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(ChatTitle))
            {
                ErrorMessage = true;
                return;
            }
            var newChat = await _chatService.StartNewChat(ChatTitle);
            PdfFile = null;
            ChatTitle = string.Empty;
            await _navigationService.GoToAsync(nameof(ChatPage), new Dictionary<string, object>
            {
                {nameof(ChatViewModel.ChatId), newChat.Id}
            });
        }
    }

    [RelayCommand]
    private void ClosePdf()
    {
        PdfFile = null;
    }

    [RelayCommand]
    private async Task GoToAllChats()
    {
        ErrorMessage = false;
        await _navigationService.GoToAsync(nameof(AllChatsPage));
    }
}
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
    private readonly IAlertService _alertService;

    public string PdfName
        => PdfFile is null ? string.Empty : PdfFile.FileName;

    public ImageSource UploadPdfIcon
        => PdfFile is null ? ImageSource.FromFile("attachment_icon") : ImageSource.FromFile("new_chat_icon");

    public string UploadPdfLabel
        => IsPdfUploading ? "Studying..." : PdfFile is null ? "Upload Pdf" : "Start new chat";

    public bool IsDiscardPdfVisible => PdfFile is not null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PdfName), nameof(UploadPdfIcon), nameof(UploadPdfLabel), nameof(IsDiscardPdfVisible))]
    private FileResult? _pdfFile;

    [ObservableProperty]
    private string _chatTitle = string.Empty;

    [ObservableProperty] 
    private bool _isErrorMessageVisible;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UploadPdfLabel))]
    private bool _isPdfUploading;
    
    public UploadViewModel(INavigationService navigationService, IChatService chatService, IAlertService alertService)
    {
        _navigationService = navigationService;
        _chatService = chatService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task ChooseFile()
    {
        IsErrorMessageVisible = false;
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
                IsErrorMessageVisible = true;
                return;
            }

            await using var pdfContent = await PdfFile.OpenReadAsync();
            IsPdfUploading = true;
            var newChat = await _chatService.StartNewChat(pdfContent, ChatTitle);
            IsPdfUploading = false;
            PdfFile = null;
            ChatTitle = string.Empty;
            await _navigationService.GoToAsync(nameof(ChatPage), new Dictionary<string, object>
            {
                {nameof(ChatViewModel.ChatId), newChat.Id}
            });
        }
    }

    [RelayCommand]
    private async Task DiscardPdf()
    {
        var shouldProceed = await _alertService.AlertAsync(
            "Discard the uploaded PDF?", 
            "This action is irreversible.",
            "Discard", 
            "Cancel");
        if (!shouldProceed) return;
        PdfFile = null;
    }

    [RelayCommand]
    private async Task GoToAllChats()
    {
        IsErrorMessageVisible = false;
        await _navigationService.GoToAsync(nameof(AllChatsPage));
    }
}
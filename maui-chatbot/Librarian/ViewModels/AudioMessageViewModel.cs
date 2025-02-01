using System.Globalization;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.Services;
using Librarian.ViewModels.Base;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace Librarian.ViewModels;

public partial class AudioMessageViewModel : ViewModelBase, IQueryAttributable
{
    private readonly INavigationService _navigationService;
    private readonly IChatService _chatService;
    
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty] 
    private string _audioQuestion = string.Empty;

    [ObservableProperty] 
    private bool _isDescriptionVisible = true;
    
    public Guid ChatId { get; set; }

    public AudioMessageViewModel(INavigationService navigationService, IChatService chatService)
    {
        _navigationService = navigationService;
        _chatService = chatService;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task StartListening()
    {
        return;
        var speech = new SpeechToTextImplementation();
        var isGranted = await speech.RequestPermissions(CancellationToken.None);
        if (!isGranted)
        {
            await Toast.Make("Permission not granted").Show(CancellationToken.None);
            return;
        }
        
        speech.RecognitionResultUpdated += DefaultOnRecognitionResultUpdated;
        speech.RecognitionResultCompleted += DefaultOnRecognitionResultCompleted;
        await speech.StartListenAsync(CultureInfo.CurrentCulture);
    }
    

    private void DefaultOnRecognitionResultUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
    {
        AudioQuestion += e.RecognitionResult;
    }

    private void DefaultOnRecognitionResultCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs e)
    {
        Console.WriteLine(e.RecognitionResult);
    }

    protected override async Task OnAppearingAsync()
    {
        var chat = await _chatService.GetCurrentChat(ChatId);
        Title = chat.Title;
        await base.OnAppearingAsync();
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(ChatId), out var chatId))
        {
            ChatId = (Guid)chatId;
        }
    }
}
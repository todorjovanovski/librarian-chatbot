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
    private readonly ISpeechToText _speechToText;
    
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(IsDescriptionVisible), nameof(VoiceIcon))]
    private string _audioQuestion = string.Empty;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(VoiceIcon))]
    private bool _isListening;
    
    public Guid ChatId { get; set; }
    public bool IsDescriptionVisible => AudioQuestion == string.Empty;

    public ImageSource VoiceIcon =>
        IsListening ? ImageSource.FromFile("voice_gif.gif") :
        AudioQuestion == string.Empty ? ImageSource.FromFile("mic_icon") : ImageSource.FromFile("confirm_icon");

    public AudioMessageViewModel(INavigationService navigationService, IChatService chatService, ISpeechToText speechToText)
    {
        _navigationService = navigationService;
        _chatService = chatService;
        _speechToText = speechToText;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync(new Dictionary<string, object>
        {
            { nameof(ChatViewModel.Question), string.Empty }
        });
    }

    [RelayCommand]
    private async Task ToggleListening()
    {
        if (IsListening)
        {
            await StopListening();
            return;
        }

        if (AudioQuestion != string.Empty)
        {
            await _navigationService.GoBackAsync(new Dictionary<string, object>
            {
                { nameof(ChatViewModel.Question), AudioQuestion }
            });
            return;
        }

        IsListening = true;
        var isGranted = await _speechToText.RequestPermissions(CancellationToken.None);
        if (!isGranted)
        {
            await Toast.Make("Permission not granted").Show(CancellationToken.None);
            return;
        }
        
        _speechToText.RecognitionResultUpdated += DefaultOnRecognitionResultUpdated;
        await _speechToText.StartListenAsync(CultureInfo.CurrentCulture);
    }

    [RelayCommand]
    private async Task ResetQuestion()
    {
        AudioQuestion = string.Empty;
        if (!IsListening) return;
        await StopListening();
    }

    protected override async Task OnAppearingAsync()
    {
        var chat = await _chatService.GetCurrentChat(ChatId);
        Title = chat.Title;
        await base.OnAppearingAsync();
    }

    protected override async Task OnDisappearingAsync()
    {
        if (IsListening)
        {
            await StopListening();
        }
        await base.OnDisappearingAsync();
    }
    
    private async Task StopListening()
    {
        await _speechToText.StopListenAsync();
        _speechToText.RecognitionResultUpdated -= DefaultOnRecognitionResultUpdated;
        IsListening = false;
    }
    
    private void DefaultOnRecognitionResultUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
    {
        AudioQuestion += $" {e.RecognitionResult}";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(ChatId), out var chatId))
        {
            ChatId = (Guid)chatId;
        }
    }
}
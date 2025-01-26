using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.Models;
using Librarian.Pages;
using Librarian.Services;
using Librarian.ViewModels.Base;

namespace Librarian.ViewModels;

public partial class ChatViewModel : ViewModelBase, IQueryAttributable
{
    private readonly INavigationService _navigationService;
    private readonly IChatService _chatService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyChatLabel))]
    private string _title = string.Empty;

    [ObservableProperty] 
    private ObservableCollection<Message> _messages = [];

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(EntryBoxIcon))]
    private string _question = string.Empty;

    [ObservableProperty] 
    private bool _isChatEmpty = true;
    
    public Guid ChatId { get; set; }
    public ImageSource EntryBoxIcon => 
        Question == string.Empty ? ImageSource.FromFile("mic_icon") : ImageSource.FromFile("send_icon");
    public string EmptyChatLabel => $"Feel free to ask me anything about {Title}.";

    public ChatViewModel(INavigationService navigationService, IChatService chatService)
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
    private async Task EntryBoxIconTapped()
    {
        if (Question != string.Empty)
        {
            var question = new Message
            {
                ChatId = ChatId,
                Content = Question,
                Sender = Sender.User
            };
            Messages.Add(question);
            await Task.Delay(1000);
            var answer = new Message
            {
                ChatId = ChatId,
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                          "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ",
                Sender = Sender.ChatBot
            };
            Messages.Add(answer);
            await _chatService.AddMessage(question);
            await _chatService.AddMessage(answer);
            Question = string.Empty;
            IsChatEmpty = false;
        }
        else
        {
            await _navigationService.GoToAsync(nameof(AudioMessagePage), new Dictionary<string, object>
            {
                {nameof(AudioMessageViewModel.ChatId), ChatId}
            });
        }
    }

    protected override async Task OnAppearingAsync()
    {
        var chat = await _chatService.GetCurrentChat(ChatId);
        Title = chat.Title;
        Messages = chat.Messages.ToObservableCollection();
        IsChatEmpty = Messages.Count == 0;
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
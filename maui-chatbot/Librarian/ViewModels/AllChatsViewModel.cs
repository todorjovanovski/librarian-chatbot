using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.Models;
using Librarian.Pages;
using Librarian.Services;
using Librarian.ViewModels.Base;

namespace Librarian.ViewModels;

public partial class AllChatsViewModel : ViewModelBase
{
    private readonly IChatService _chatService;
    private readonly INavigationService _navigationService;
    
    [ObservableProperty] 
    private ObservableCollection<Chat> _chats = [];

    [ObservableProperty] 
    private bool _isChatsEmpty;

    public AllChatsViewModel(IChatService chatService, INavigationService navigationService)
    {
        _chatService = chatService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task GoToChat(Guid chatId)
    {
        await _navigationService.GoToAsync(nameof(ChatPage), new Dictionary<string, object>
        {
            { nameof(ChatViewModel.ChatId), chatId }
        });
    }

    [RelayCommand]
    private async Task DeleteChat(Guid chatId)
    {
        await _chatService.DeleteChat(chatId);
        var chats = await _chatService.GetAllChats();
        chats.Reverse();
        Chats = chats.ToObservableCollection();
        IsChatsEmpty = Chats.Count == 0;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync();
    }

    protected override async Task OnAppearingAsync()
    {
        var chats = await _chatService.GetAllChats();
        chats.Reverse();
        Chats = chats.ToObservableCollection();
        IsChatsEmpty = Chats.Count == 0;
        await base.OnAppearingAsync();
    }
}
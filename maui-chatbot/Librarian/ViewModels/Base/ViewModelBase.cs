using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Librarian.Services;

namespace Librarian.ViewModels.Base;

public partial class ViewModelBase : ObservableObject, IViewModelBase
{
    [ObservableProperty] 
    private bool _isLoading;

    public IAsyncRelayCommand OnAppearingAsyncCommand { get; }
    public IAsyncRelayCommand OnDisappearingAsyncCommand { get; }
    public IAsyncRelayCommand OnSleepAsyncCommand { get; }
    public IAsyncRelayCommand OnResumeAsyncCommand { get; }
    
    public ViewModelBase()
    {
        OnAppearingAsyncCommand = new AsyncRelayCommand(async () =>
        {
            IsLoading = true;
            await Appearing(OnAppearingAsync);
            IsLoading = false;
        });
        OnDisappearingAsyncCommand = new AsyncRelayCommand(async () => await Disappearing(OnDisappearingAsync));
        OnSleepAsyncCommand = new AsyncRelayCommand(async () => await Sleeping(OnSleepAsync));
        OnResumeAsyncCommand = new AsyncRelayCommand(async () => await Resuming(OnResumeAsync));
    }
    
    private async Task Appearing(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }

    protected virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    private async Task Disappearing(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }

    protected virtual Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }

    private async Task Sleeping(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }

    protected virtual Task OnSleepAsync()
    {
        return Task.CompletedTask;
    }

    private async Task Resuming(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }

    protected virtual Task OnResumeAsync()
    {
        return Task.CompletedTask;
    }
}
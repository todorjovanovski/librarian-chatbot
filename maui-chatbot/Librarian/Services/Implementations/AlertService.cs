namespace Librarian.Services.Implementations;

public class AlertService : IAlertService
{
    public Task AlertAsync(string title, string message, string cancel)
        => MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.DisplayAlert(title, message, cancel));

    public async Task<bool> AlertAsync(string title, string message, string accept, string cancel)
    {
        var isAccepted = false;
        
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            isAccepted = await Shell.Current.DisplayAlert(title, message, accept, cancel);
        });

        return isAccepted;
    }
}
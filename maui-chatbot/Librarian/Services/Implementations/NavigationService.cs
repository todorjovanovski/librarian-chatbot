namespace Librarian.Services.Implementations;

public class NavigationService : INavigationService
{
    public Task GoToAsync(string uri)
        => MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(uri));

    public Task GoToAsync(string uri, Dictionary<string, object> parameters)
        => MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(uri, parameters));

    public Task GoBackAsync() 
        => MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(".."));

    public Task GoBackAsync(Dictionary<string, object> parameters) 
        => MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("..", parameters));
}
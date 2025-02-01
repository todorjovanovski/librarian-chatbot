namespace Librarian.Services;

public interface INavigationService
{
    public Task GoToAsync(string uri);
    public Task GoToAsync(string uri, Dictionary<string, object> parameters);
    public Task GoBackAsync();
    public Task GoBackAsync(Dictionary<string, object> parameters);
}
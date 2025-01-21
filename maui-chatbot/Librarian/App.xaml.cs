using Librarian.Pages;
using Librarian.Repository;

namespace Librarian;

public partial class App : Application
{
    private readonly IRepository _repository;
    private ContentPageBase? CurrentPage { get; set; }
    
    public App(IRepository repository)
    {
        _repository = repository;
        InitializeComponent();
        MainPage = new AppShell();
        UserAppTheme = AppTheme.Light;
        PageAppearing += OnPageAppearing;
    }

    private void OnPageAppearing(object? sender, Page page)
    {
        if (page is ContentPageBase currentPage)
        {
            CurrentPage = currentPage;
        }
    }

    protected override async void OnSleep()
    {
        if (CurrentPage is not null)
        {
            await CurrentPage.OnSleep();
        }
        base.OnSleep();
    }
    
    protected override async void OnResume()
    {
        if (CurrentPage is not null)
        {
            await CurrentPage.OnResume();
        }
        base.OnResume();
    }

    protected override async void OnStart()
    {
        await _repository.InitializeDatabase();
        base.OnStart();
    }
}
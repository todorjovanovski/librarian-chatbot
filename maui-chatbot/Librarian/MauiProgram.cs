using CommunityToolkit.Maui;
using Librarian.Pages;
using Librarian.Repository;
using Librarian.Services;
using Librarian.Services.Implementations;
using Librarian.ViewModels;
using Microsoft.Extensions.Logging;

namespace Librarian;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Koulen-Regular.ttf", "KoulenRegular");
            })
            .RegisterServices()
            .RegisterRepositories()
            .RegisterPagesWithViewModels();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IChatService, ChatService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        return builder;
    }

    private static MauiAppBuilder RegisterRepositories(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IRepository, Repository.Implementations.Repository>();

        return builder;
    }

    private static void RegisterPagesWithViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<UploadPage>();
        builder.Services.AddSingleton<UploadViewModel>();
        builder.Services.AddTransientWithShellRoute<ChatPage, ChatViewModel>(nameof(ChatPage));
        builder.Services.AddTransientWithShellRoute<AudioMessagePage, AudioMessageViewModel>(nameof(AudioMessagePage));
        builder.Services.AddTransientWithShellRoute<AllChatsPage, AllChatsViewModel>(nameof(AllChatsPage));

    }
}
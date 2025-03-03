using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Librarian.ViewModels.Base;
using Librarian.ViewModels.Base;

namespace Librarian.Pages;

public partial class ContentPageBase : ContentPage
{
    public ContentPageBase()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    protected override async void OnAppearing()
    {
        if (BindingContext is IViewModelBase viewModelBase)
        {
            await viewModelBase.OnAppearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnAppearing();
    }

    protected override async void OnDisappearing()
    {
        if (BindingContext is IViewModelBase viewModelBase)
        {
            await viewModelBase.OnDisappearingAsyncCommand.ExecuteAsync(null);
        }
        base.OnDisappearing();
    }

    public virtual async Task OnSleep()
    {
        if (BindingContext is IViewModelBase viewModelBase)
        {
            await viewModelBase.OnSleepAsyncCommand.ExecuteAsync(null);
        }
    }

    public virtual async Task OnResume()
    {
        if (BindingContext is IViewModelBase viewModelBase)
        {
            await viewModelBase.OnResumeAsyncCommand.ExecuteAsync(null);
        }
    }
}
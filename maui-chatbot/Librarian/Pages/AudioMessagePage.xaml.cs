using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Librarian.ViewModels;

namespace Librarian.Pages;

public partial class AudioMessagePage : ContentPageBase
{
    public AudioMessagePage(AudioMessageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
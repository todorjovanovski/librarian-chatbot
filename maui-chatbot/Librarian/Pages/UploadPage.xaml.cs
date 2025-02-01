using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Librarian.ViewModels;

namespace Librarian.Pages;

public partial class UploadPage : ContentPageBase
{
    public UploadPage(UploadViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
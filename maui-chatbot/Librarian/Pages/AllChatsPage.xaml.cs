using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Librarian.ViewModels;

namespace Librarian.Pages;

public partial class AllChatsPage : ContentPageBase
{
    public AllChatsPage(AllChatsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
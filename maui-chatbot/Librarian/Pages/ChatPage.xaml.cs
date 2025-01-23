using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Librarian.ViewModels;

namespace Librarian.Pages;

public partial class ChatPage : ContentPageBase
{
    public ChatPage(ChatViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
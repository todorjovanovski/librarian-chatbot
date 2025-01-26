using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Librarian.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Librarian.Views;

public partial class MessageView : ContentView
{
    public MessageView()
    {
        InitializeComponent();
        OnSenderChanging(this, new object(), MessageSender);
    }
    
    public static readonly BindableProperty MessageTextProperty =
        BindableProperty.Create(nameof(MessageText), typeof(string), typeof(MessageView));

    public string MessageText
    {
        get => (string)GetValue(MessageTextProperty);
        set => SetValue(MessageTextProperty, value);
    }
    
    public static readonly BindableProperty MessageSenderProperty =
        BindableProperty.Create(nameof(MessageSender), typeof(Sender), typeof(MessageView), propertyChanging: OnSenderChanging);

    
    public Sender MessageSender
    {
        get => (Sender)GetValue(MessageSenderProperty);
        set => SetValue(MessageSenderProperty, value);
    }
    
    public static readonly BindableProperty DateSentProperty =
        BindableProperty.Create(nameof(DateSent), typeof(DateTime), typeof(MessageView));

    public DateTime DateSent
    {
        get => (DateTime)GetValue(DateSentProperty);
        set => SetValue(DateSentProperty, value.ToString("hh:mm"));
    }
    
    public static readonly BindableProperty IsDateSentVisibleProperty =
        BindableProperty.Create(nameof(IsDateSentVisible), typeof(bool), typeof(MessageView));

    public bool IsDateSentVisible
    {
        get => (bool)GetValue(IsDateSentVisibleProperty);
        set => SetValue(IsDateSentVisibleProperty, value);
    }
    
    private static void OnSenderChanging(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (MessageView)bindable;
        var sender = (Sender)newValue;
        if (sender == Sender.User)
        {
            view.MessageBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10, 10, 10, 0) };
            view.MessageTextLabel.HorizontalTextAlignment = TextAlignment.End;
            view.MessageStackLayout.HorizontalOptions = LayoutOptions.End;
            view.MessageStackLayout.Padding = new Thickness(30, 0, 0, 0);
            view.DateSentLabel.HorizontalTextAlignment = TextAlignment.End;
        }
        else
        {
            view.MessageBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10, 10, 0, 10) };
            view.MessageTextLabel.HorizontalTextAlignment = TextAlignment.Start;
            view.MessageStackLayout.HorizontalOptions = LayoutOptions.Start;
            view.MessageStackLayout.Padding = new Thickness(0, 0, 30, 0);
            view.DateSentLabel.HorizontalTextAlignment = TextAlignment.Start;
        }
    }

    private void OnMessageTapped(object? sender, TappedEventArgs e)
    {
        DateSentLabel.Text = DateSent.Hour + ":" + DateSent.Minute;
        DateSentLabel.FontSize = 12;
        DateSentLabel.TextColor = Colors.DimGray;
        DateSentLabel.Padding = new Thickness(3);
        IsDateSentVisible = !IsDateSentVisible;
    }
}
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace PLinkageApp.Controls;

public partial class ChatPreviewCard : ContentView
    {
        public static readonly BindableProperty SenderNameProperty =
            BindableProperty.Create(nameof(SenderName), typeof(string), typeof(ChatPreviewCard), string.Empty);

        public static readonly BindableProperty MessageTextProperty =
            BindableProperty.Create(nameof(MessageText), typeof(string), typeof(ChatPreviewCard), string.Empty);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ChatPreviewCard), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ChatPreviewCard), null);


        public string SenderName
        {
            get => (string)GetValue(SenderNameProperty);
            set => SetValue(SenderNameProperty, value);
        }

        public string MessageText
        {
            get => (string)GetValue(MessageTextProperty);
            set => SetValue(MessageTextProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ChatPreviewCard()
        {
            InitializeComponent();
        }
        
    }
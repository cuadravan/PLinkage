using System.Windows.Input;

namespace PLinkageApp.WindowsControls;

public partial class ProjectOwnerCard : ContentView
{
    public static readonly BindableProperty UserNameProperty =
        BindableProperty.Create(nameof(UserName), typeof(string), typeof(ProjectOwnerCard), string.Empty);

    public static readonly BindableProperty UserStatusProperty =
        BindableProperty.Create(nameof(UserStatus), typeof(string), typeof(ProjectOwnerCard), string.Empty);

    public static readonly BindableProperty ProjectCountProperty =
        BindableProperty.Create(nameof(ProjectCount), typeof(string), typeof(ProjectOwnerCard), string.Empty);

    public string UserName
    {
        get => (string)GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public string UserStatus
    {
        get => (string)GetValue(UserStatusProperty);
        set => SetValue(UserStatusProperty, value);
    }

    public string ProjectCount
    {
        get => (string)GetValue(ProjectCountProperty);
        set => SetValue(ProjectCountProperty, value);
    }
    

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ProjectOwnerCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ProjectCard), null);

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

    public ProjectOwnerCard()
    {
        InitializeComponent();
    }
}
using System.Windows.Input;

namespace PLinkageApp.AndroidControls;

public partial class ProfileProjectCard : ContentView
{
    public static readonly BindableProperty IsOwnerProperty =
        BindableProperty.Create(nameof(IsOwner), typeof(bool), typeof(ProfileProjectCard), false);

    public bool IsOwner
    {
        get => (bool)GetValue(IsOwnerProperty);
        set => SetValue(IsOwnerProperty, value);
    }


    public static readonly BindableProperty ProjectNameProperty =
        BindableProperty.Create(nameof(ProjectName), typeof(string), typeof(ProfileProjectCard), string.Empty);

    public string ProjectName
    {
        get => (string)GetValue(ProjectNameProperty);
        set => SetValue(ProjectNameProperty, value);
    }

    public static readonly BindableProperty ProjectStatusProperty =
        BindableProperty.Create(nameof(ProjectStatus), typeof(string), typeof(ProfileProjectCard), string.Empty);

    public string ProjectStatus
    {
        get => (string)GetValue(ProjectStatusProperty);
        set => SetValue(ProjectStatusProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ProfileProjectCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ProfileProjectCard), null);

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
    public ProfileProjectCard()
	{
		InitializeComponent();
	}
}
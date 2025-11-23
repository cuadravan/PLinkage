using System.Windows.Input;

namespace PLinkageApp.WindowsControls;

public partial class ResignSkillProviderCard : ContentView
{
	public ResignSkillProviderCard()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty FullNameProperty =
        BindableProperty.Create(nameof(FullName), typeof(string), typeof(ResignSkillProviderCard), string.Empty);

    public string FullName
    {
        get => (string)GetValue(FullNameProperty);
        set => SetValue(FullNameProperty, value);
    }

    public static readonly BindableProperty ProjectNameProperty =
        BindableProperty.Create(nameof(ProjectName), typeof(string), typeof(ResignSkillProviderCard), string.Empty);

    public string ProjectName
    {
        get => (string)GetValue(ProjectNameProperty);
        set => SetValue(ProjectNameProperty, value);
    }

    public static readonly BindableProperty ReasonProperty =
        BindableProperty.Create(nameof(Reason), typeof(string), typeof(ResignSkillProviderCard), string.Empty);

    public string Reason
    {
        get => (string)GetValue(ReasonProperty);
        set => SetValue(ReasonProperty, value);
    }

    public static readonly BindableProperty ApproveCommandProperty =
            BindableProperty.Create(nameof(ApproveCommand), typeof(ICommand), typeof(ResignSkillProviderCard), null);

    public ICommand ApproveCommand
    {
        get => (ICommand)GetValue(ApproveCommandProperty);
        set => SetValue(ApproveCommandProperty, value);
    }

    public static readonly BindableProperty RejectCommandProperty =
            BindableProperty.Create(nameof(RejectCommand), typeof(ICommand), typeof(ResignSkillProviderCard), null);

    public ICommand RejectCommand
    {
        get => (ICommand)GetValue(RejectCommandProperty);
        set => SetValue(RejectCommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ResignSkillProviderCard), null);
}
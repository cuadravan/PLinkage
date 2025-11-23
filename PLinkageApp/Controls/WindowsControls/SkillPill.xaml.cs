using System.Windows.Input;

namespace PLinkageApp.WindowsControls;

public partial class SkillPill : ContentView
{
    public static readonly BindableProperty SkillNameProperty =
        BindableProperty.Create(nameof(SkillName), typeof(string), typeof(SkillPill), string.Empty);

    public string SkillName
    {
        get => (string)GetValue(SkillNameProperty);
        set => SetValue(SkillNameProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SkillPill), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SkillPill), null);

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

    public SkillPill()
	{
		InitializeComponent();
	}
}
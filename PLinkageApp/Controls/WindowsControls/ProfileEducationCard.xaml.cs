using System.Windows.Input;

namespace PLinkageApp.WindowsControls;

public partial class ProfileEducationCard : ContentView
{
    public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ProfileEducationCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ProfileEducationCard), null);

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

    public static readonly BindableProperty SchoolAttendedProperty =
        BindableProperty.Create(nameof(SchoolAttended), typeof(string), typeof(ProfileEducationCard), string.Empty);

    public string SchoolAttended
    {
        get => (string)GetValue(SchoolAttendedProperty);
        set => SetValue(SchoolAttendedProperty, value);
    }

    public static readonly BindableProperty CourseNameProperty =
        BindableProperty.Create(nameof(CourseName), typeof(string), typeof(ProfileEducationCard), string.Empty);

    public string CourseName
    {
        get => (string)GetValue(CourseNameProperty);
        set => SetValue(CourseNameProperty, value);
    }

    public static readonly BindableProperty TimeGraduatedProperty =
        BindableProperty.Create(nameof(TimeGraduated), typeof(DateTime), typeof(ProfileEducationCard), DateTime.Now);

    public DateTime TimeGraduated
    {
        get => (DateTime)GetValue(TimeGraduatedProperty);
        set => SetValue(TimeGraduatedProperty, value);
    }

    public ProfileEducationCard()
	{
		InitializeComponent();
	}
}
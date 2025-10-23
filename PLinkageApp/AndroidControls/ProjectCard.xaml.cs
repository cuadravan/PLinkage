using System.Windows.Input;

namespace PLinkageApp.AndroidControls;

public partial class ProjectCard : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ProjectCard), string.Empty);

    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(string), typeof(ProjectCard), string.Empty);

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(ProjectCard), string.Empty);

    public static readonly BindableProperty SkillsProperty =
        BindableProperty.Create(nameof(Skills), typeof(IEnumerable<string>), typeof(ProjectCard), Enumerable.Empty<string>());

    public static readonly BindableProperty SlotsProperty =
        BindableProperty.Create(nameof(Slots), typeof(string), typeof(ProjectCard), string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public string Slots
    {
        get => (string)GetValue(SlotsProperty);
        set => SetValue(SlotsProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public IEnumerable<string> Skills
    {
        get => (IEnumerable<string>)GetValue(SkillsProperty);
        set => SetValue(SkillsProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ProjectCard), null);

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

    public ProjectCard()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
namespace PLinkageApp.AndroidControls;

public partial class SkillProviderCard : ContentView
{
    public static readonly BindableProperty UserNameProperty =
        BindableProperty.Create(nameof(UserName), typeof(string), typeof(SkillProviderCard), string.Empty);

    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(string), typeof(SkillProviderCard), string.Empty);

    public static readonly BindableProperty EducationProperty =
        BindableProperty.Create(nameof(Education), typeof(string), typeof(SkillProviderCard), string.Empty);

    public static readonly BindableProperty SkillsProperty =
        BindableProperty.Create(nameof(Skills), typeof(IEnumerable<string>), typeof(SkillProviderCard), Enumerable.Empty<string>());

    public static readonly BindableProperty UserRatingProperty =
        BindableProperty.Create(nameof(UserRating), typeof(string), typeof(SkillProviderCard), string.Empty);

    public string UserName
    {
        get => (string)GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public string UserRating
    {
        get => (string)GetValue(UserRatingProperty);
        set => SetValue(UserRatingProperty, value);
    }

    public string Education
    {
        get => (string)GetValue(EducationProperty);
        set => SetValue(EducationProperty, value);
    }

    public IEnumerable<string> Skills
    {
        get => (IEnumerable<string>)GetValue(SkillsProperty);
        set => SetValue(SkillsProperty, value);
    }

    public SkillProviderCard()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
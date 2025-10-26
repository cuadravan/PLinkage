namespace PLinkageApp.AndroidControls;

public partial class SkillPill : ContentView
{
    public static readonly BindableProperty SkillNameProperty =
        BindableProperty.Create(nameof(SkillName), typeof(string), typeof(ProfileEducationCard), string.Empty);

    public string SkillName
    {
        get => (string)GetValue(SkillNameProperty);
        set => SetValue(SkillNameProperty, value);
    }
    public SkillPill()
	{
		InitializeComponent();
	}
}
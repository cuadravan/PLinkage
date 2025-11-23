namespace PLinkageApp.WindowsControls;

public partial class RateSkillProviderCard : ContentView
{
	public RateSkillProviderCard()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty FullNameProperty =
        BindableProperty.Create(nameof(FullName), typeof(string), typeof(RateSkillProviderCard), string.Empty);

    public string FullName
    {
        get => (string)GetValue(FullNameProperty);
        set => SetValue(FullNameProperty, value);
    }

    public static readonly BindableProperty RatingProperty =
        BindableProperty.Create(nameof(Rating), typeof(double), typeof(RateSkillProviderCard), 0.0, defaultBindingMode: BindingMode.TwoWay);

    public double Rating
    {
        get => (double)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }
}
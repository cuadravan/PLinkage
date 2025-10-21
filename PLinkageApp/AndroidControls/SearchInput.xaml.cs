namespace PLinkageApp.AndroidControls;

public partial class SearchInput : ContentView
{
    public static readonly BindableProperty SearchPlaceholderProperty =
        BindableProperty.Create(nameof(SearchPlaceholder), typeof(string), typeof(SearchInput), "Search...");

    public string SearchPlaceholder
    {
        get => (string)GetValue(SearchPlaceholderProperty);
        set => SetValue(SearchPlaceholderProperty, value);
    }

    public static readonly BindableProperty SearchQueryProperty =
        BindableProperty.Create(nameof(SearchQuery), typeof(string), typeof(SearchInput), string.Empty, BindingMode.TwoWay);

    public string SearchQuery
    {
        get => (string)GetValue(SearchQueryProperty);
        set => SetValue(SearchQueryProperty, value);
    }

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(string), typeof(SearchInput), "search.svg");

    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public static readonly BindableProperty ControlWidthProperty =
        BindableProperty.Create(nameof(ControlWidth), typeof(double), typeof(SearchInput), 200.0);

    public double ControlWidth
    {
        get => (double)GetValue(ControlWidthProperty);
        set => SetValue(ControlWidthProperty, value);
    }

    public SearchInput()
    {
        InitializeComponent();
    }
}

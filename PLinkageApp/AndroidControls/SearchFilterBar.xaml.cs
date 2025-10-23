using System.Collections;

namespace PLinkageApp.AndroidControls;

public partial class SearchFilterBar : ContentView
{
    public static readonly BindableProperty SearchPlaceholderProperty =
        BindableProperty.Create(nameof(SearchPlaceholder), typeof(string), typeof(SearchFilterBar), "Search...");

    public string SearchPlaceholder
    {
        get => (string)GetValue(SearchPlaceholderProperty);
        set => SetValue(SearchPlaceholderProperty, value);
    }

    public static readonly BindableProperty SearchIconSourceProperty =
        BindableProperty.Create(nameof(SearchIconSource), typeof(string), typeof(SearchFilterBar), "search.svg");

    public string SearchIconSource
    {
        get => (string)GetValue(SearchIconSourceProperty);
        set => SetValue(SearchIconSourceProperty, value);
    }

    public static readonly BindableProperty SearchQueryProperty =
        BindableProperty.Create(nameof(SearchQuery), typeof(string), typeof(SearchFilterBar), string.Empty, BindingMode.TwoWay);

    public string SearchQuery
    {
        get => (string)GetValue(SearchQueryProperty);
        set => SetValue(SearchQueryProperty, value);
    }

    public static readonly BindableProperty SearchWidthProperty =
        BindableProperty.Create(nameof(SearchWidth), typeof(double), typeof(SearchFilterBar), 200.0);

    public double SearchWidth
    {
        get => (double)GetValue(SearchWidthProperty);
        set => SetValue(SearchWidthProperty, value);
    }

    public static readonly BindableProperty FilterTitleProperty =
        BindableProperty.Create(nameof(FilterTitle), typeof(string), typeof(SearchFilterBar), "Filter");

    public string FilterTitle
    {
        get => (string)GetValue(FilterTitleProperty);
        set => SetValue(FilterTitleProperty, value);
    }

    public static readonly BindableProperty FilterItemSourceProperty =
        BindableProperty.Create(nameof(FilterItemSource), typeof(IEnumerable), typeof(SearchFilterBar), null);

    public IEnumerable FilterItemSource
    {
        get => (IEnumerable)GetValue(FilterItemSourceProperty);
        set => SetValue(FilterItemSourceProperty, value);
    }

    public static readonly BindableProperty FilterSelectedItemProperty =
        BindableProperty.Create(nameof(FilterSelectedItem), typeof(object), typeof(SearchFilterBar), null, BindingMode.TwoWay);

    public object FilterSelectedItem
    {
        get => GetValue(FilterSelectedItemProperty);
        set => SetValue(FilterSelectedItemProperty, value);
    }

    public static readonly BindableProperty FilterWidthProperty =
        BindableProperty.Create(nameof(FilterWidth), typeof(double), typeof(SearchFilterBar), 100.0);

    public double FilterWidth
    {
        get => (double)GetValue(FilterWidthProperty);
        set => SetValue(FilterWidthProperty, value);
    }


    public SearchFilterBar()
    {
        InitializeComponent();
    }
}

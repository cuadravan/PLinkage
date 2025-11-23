using System.Collections;

namespace PLinkageApp.AndroidControls;

public partial class FilterButton : ContentView
{
    public static readonly BindableProperty FilterTitleProperty =
        BindableProperty.Create(nameof(FilterTitle), typeof(string), typeof(FilterButton), "Filter");

    public string FilterTitle
    {
        get => (string)GetValue(FilterTitleProperty);
        set => SetValue(FilterTitleProperty, value);
    }

    public static readonly BindableProperty FilterItemSourceProperty =
        BindableProperty.Create(nameof(FilterItemSource), typeof(IEnumerable), typeof(FilterButton), null);

    public IEnumerable FilterItemSource
    {
        get => (IEnumerable)GetValue(FilterItemSourceProperty);
        set => SetValue(FilterItemSourceProperty, value);
    }

    public static readonly BindableProperty FilterSelectedItemProperty =
        BindableProperty.Create(nameof(FilterSelectedItem), typeof(object), typeof(FilterButton), null, BindingMode.TwoWay);

    public object FilterSelectedItem
    {
        get => GetValue(FilterSelectedItemProperty);
        set => SetValue(FilterSelectedItemProperty, value);
    }

    public static readonly BindableProperty ControlWidthProperty =
        BindableProperty.Create(nameof(ControlWidth), typeof(double), typeof(FilterButton), 100.0);

    public double ControlWidth
    {
        get => (double)GetValue(ControlWidthProperty);
        set => SetValue(ControlWidthProperty, value);
    }

    public FilterButton()
    {
        InitializeComponent();
    }
}

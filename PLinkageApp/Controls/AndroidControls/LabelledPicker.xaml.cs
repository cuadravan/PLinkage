using System.Collections;

namespace PLinkageApp.AndroidControls;

public partial class LabeledPicker : ContentView
{
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(LabeledPicker), string.Empty);

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(LabeledPicker), "Select Option");

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly BindableProperty ItemSourceProperty =
        BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable), typeof(LabeledPicker), null);

    public IEnumerable ItemSource
    {
        get => (IEnumerable)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(LabeledPicker), null, BindingMode.TwoWay);

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty ControlWidthProperty =
        BindableProperty.Create(nameof(ControlWidth), typeof(double), typeof(LabeledPicker), 280.0);

    public double ControlWidth
    {
        get => (double)GetValue(ControlWidthProperty);
        set => SetValue(ControlWidthProperty, value);
    }

    public LabeledPicker()
    {
        InitializeComponent();
    }
}

using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace PLinkageApp.Controls
{
    public partial class SegmentedControl : ContentView
    {
        public SegmentedControl()
        {
            InitializeComponent();
            Segments = new ObservableCollection<string>();
        }

        public static readonly BindableProperty SegmentsProperty =
            BindableProperty.Create(nameof(Segments), typeof(ObservableCollection<string>),
                typeof(SegmentedControl), new ObservableCollection<string>());

        public ObservableCollection<string> Segments
        {
            get => (ObservableCollection<string>)GetValue(SegmentsProperty);
            set => SetValue(SegmentsProperty, value);
        }

        public static readonly BindableProperty SelectedSegmentProperty =
            BindableProperty.Create(nameof(SelectedSegment), typeof(string),
                typeof(SegmentedControl), string.Empty, BindingMode.TwoWay);

        public string SelectedSegment
        {
            get => (string)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }
    }
}

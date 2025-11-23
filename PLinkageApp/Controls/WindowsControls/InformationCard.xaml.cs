using Microsoft.Maui.Controls;

namespace PLinkageApp.WindowsControls
{
    public partial class InformationCard : ContentView
    {
        // BindableProperty for the "Title"
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(InformationCard), "Information"); // Default value is "Information"

        // BindableProperty for "Gender"
        public static readonly BindableProperty GenderProperty =
            BindableProperty.Create(nameof(Gender), typeof(string), typeof(InformationCard), string.Empty);

        // BindableProperty for "Email"
        public static readonly BindableProperty EmailProperty =
            BindableProperty.Create(nameof(Email), typeof(string), typeof(InformationCard), string.Empty);

        // BindableProperty for "Mobile"
        public static readonly BindableProperty MobileProperty =
            BindableProperty.Create(nameof(Mobile), typeof(string), typeof(InformationCard), string.Empty);


        // --- CLR Wrappers for the properties ---

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Gender
        {
            get => (string)GetValue(GenderProperty);
            set => SetValue(GenderProperty, value);
        }

        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }

        public string Mobile
        {
            get => (string)GetValue(MobileProperty);
            set => SetValue(MobileProperty, value);
        }

        public InformationCard()
        {
            InitializeComponent();
        }
    }
}

using System.Windows.Input;

namespace PLinkageApp.WindowsControls;

public partial class OfferApplicationCard : ContentView
{
    public static readonly BindableProperty ProjectNameProperty =
        BindableProperty.Create(nameof(ProjectName), typeof(string), typeof(OfferApplicationCard));

    public string ProjectName
    {
        get => (string)GetValue(ProjectNameProperty);
        set => SetValue(ProjectNameProperty, value);
    }

    public static readonly BindableProperty FormattedConcernedNameProperty =
        BindableProperty.Create(nameof(FormattedConcernedName), typeof(string), typeof(OfferApplicationCard));

    public string FormattedConcernedName
    {
        get => (string)GetValue(FormattedConcernedNameProperty);
        set => SetValue(FormattedConcernedNameProperty, value);
    }

    public static readonly BindableProperty OfferApplicationTypeProperty =
        BindableProperty.Create(nameof(OfferApplicationType), typeof(string), typeof(OfferApplicationCard),
        propertyChanged: OnTypeChanged); // <-- MODIFIED: Added propertyChanged

    public string OfferApplicationType
    {
        get => (string)GetValue(OfferApplicationTypeProperty);
        set => SetValue(OfferApplicationTypeProperty, value);
    }

    // Color derived from Type
    public static readonly BindableProperty TypeColorProperty =
        BindableProperty.Create(nameof(TypeColor), typeof(Color), typeof(OfferApplicationCard), Colors.Gray); // <-- NEW

    public Color TypeColor
    {
        get => (Color)GetValue(TypeColorProperty);
        set => SetValue(TypeColorProperty, value);
    }

    private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue) // <-- NEW
    {
        if (bindable is OfferApplicationCard view && newValue is string type)
        {
            view.TypeColor = type switch
            {
                "Offer" => Color.FromArgb("#3B82F6"),
                "Application" => Color.FromArgb("#F97316"),
                _ => Colors.Gray
            };
        }
    }


    // --- Status Properties (MODIFIED) ---

    public static readonly BindableProperty OfferApplicationStatusProperty =
        BindableProperty.Create(nameof(OfferApplicationStatus), typeof(string), typeof(OfferApplicationCard), defaultValue: string.Empty,
        propertyChanged: OnStatusChanged); // (No change here, just for context)

    public string OfferApplicationStatus
    {
        get => (string)GetValue(OfferApplicationStatusProperty);
        set => SetValue(OfferApplicationStatusProperty, value);
    }

    // Color derived from status
    public static readonly BindableProperty StatusBackgroundColorProperty = // <-- MODIFIED: Renamed from StatusColor
        BindableProperty.Create(nameof(StatusBackgroundColor), typeof(Color), typeof(OfferApplicationCard), Colors.Gray);

    public Color StatusBackgroundColor
    {
        get => (Color)GetValue(StatusBackgroundColorProperty);
        set => SetValue(StatusBackgroundColorProperty, value);
    }

    public static readonly BindableProperty StatusTextColorProperty = // <-- NEW
        BindableProperty.Create(nameof(StatusTextColor), typeof(Color), typeof(OfferApplicationCard), Colors.White);

    public Color StatusTextColor
    {
        get => (Color)GetValue(StatusTextColorProperty);
        set => SetValue(StatusTextColorProperty, value);
    }

    private static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue) // <-- MODIFIED
    {
        if (bindable is OfferApplicationCard view && newValue is string status)
        {
            (Color Background, Color Text) colors = status switch
            {
                "Accepted" => (Color.FromArgb("#DCFCE7"), Color.FromArgb("#15823D")),
                "Declined" => (Color.FromArgb("#FEC3C4"), Color.FromArgb("#FEC3C4")),
                "Pending" => (Color.FromArgb("#FEF9C3"), Color.FromArgb("#A16207")), // Goldenrod has better contrast with black
                _ => (Colors.Gray, Colors.White)
            };

            view.StatusBackgroundColor = colors.Background;
            view.StatusTextColor = colors.Text;
        }
    }

    public static readonly BindableProperty FormattedRateProperty =
        BindableProperty.Create(nameof(FormattedRate), typeof(string), typeof(OfferApplicationCard));

    public string FormattedRate
    {
        get => (string)GetValue(FormattedRateProperty);
        set => SetValue(FormattedRateProperty, value);
    }

    public static readonly BindableProperty FormattedTimeFrameProperty =
        BindableProperty.Create(nameof(FormattedTimeFrame), typeof(string), typeof(OfferApplicationCard));

    public string FormattedTimeFrame
    {
        get => (string)GetValue(FormattedTimeFrameProperty);
        set => SetValue(FormattedTimeFrameProperty, value);
    }

    public static readonly BindableProperty IsNegotiatingProperty =
        BindableProperty.Create(nameof(IsNegotiating), typeof(bool), typeof(OfferApplicationCard));

    public bool IsNegotiating
    {
        get => (bool)GetValue(IsNegotiatingProperty);
        set => SetValue(IsNegotiatingProperty, value);
    }

    public static readonly BindableProperty IsNegotiableProperty =
        BindableProperty.Create(nameof(IsNegotiable), typeof(bool), typeof(OfferApplicationCard));

    public bool IsNegotiable
    {
        get => (bool)GetValue(IsNegotiableProperty);
        set => SetValue(IsNegotiableProperty, value);
    }

    public static readonly BindableProperty AwaitingResponseProperty =
        BindableProperty.Create(nameof(AwaitingResponse), typeof(bool), typeof(OfferApplicationCard));

    public bool AwaitingResponse
    {
        get => (bool)GetValue(AwaitingResponseProperty);
        set => SetValue(AwaitingResponseProperty, value);
    }

    // ============ Commands ============

    public static readonly BindableProperty AcceptCommandProperty =
        BindableProperty.Create(nameof(AcceptCommand), typeof(ICommand), typeof(OfferApplicationCard));

    public ICommand AcceptCommand
    {
        get => (ICommand)GetValue(AcceptCommandProperty);
        set => SetValue(AcceptCommandProperty, value);
    }

    public static readonly BindableProperty DeclineCommandProperty =
        BindableProperty.Create(nameof(DeclineCommand), typeof(ICommand), typeof(OfferApplicationCard));

    public ICommand DeclineCommand
    {
        get => (ICommand)GetValue(DeclineCommandProperty);
        set => SetValue(DeclineCommandProperty, value);
    }

    public static readonly BindableProperty NegotiateCommandProperty =
        BindableProperty.Create(nameof(NegotiateCommand), typeof(ICommand), typeof(OfferApplicationCard));

    public ICommand NegotiateCommand
    {
        get => (ICommand)GetValue(NegotiateCommandProperty);
        set => SetValue(NegotiateCommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(OfferApplicationCard), null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty ViewProjectCommandProperty =
        BindableProperty.Create(nameof(ViewProjectCommand), typeof(ICommand), typeof(OfferApplicationCard));

    public ICommand ViewProjectCommand
    {
        get => (ICommand)GetValue(ViewProjectCommandProperty);
        set => SetValue(ViewProjectCommandProperty, value);
    }

    public static readonly BindableProperty ViewConcernedCommandProperty =
        BindableProperty.Create(nameof(ViewConcernedCommand), typeof(ICommand), typeof(OfferApplicationCard));

    public ICommand ViewConcernedCommand
    {
        get => (ICommand)GetValue(ViewConcernedCommandProperty);
        set => SetValue(ViewConcernedCommandProperty, value);
    }

    public OfferApplicationCard()
    {
        InitializeComponent();
    }
}
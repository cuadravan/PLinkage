namespace PLinkage
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ServiceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Return a temporary window with the splash screen
            return new Window(new SplashScreenPage());
        }
    }
}

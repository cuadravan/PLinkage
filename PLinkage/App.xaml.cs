namespace PLinkage
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Get the service provider from the current MauiApp instance
            var serviceProvider = IPlatformApplication.Current.Services;

            // Resolve AppShell from the DI container
            var appShell = serviceProvider.GetService<AppShell>();

            return new Window(appShell);
        }
    }
}
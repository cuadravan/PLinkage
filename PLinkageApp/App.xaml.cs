using PLinkageApp.Interfaces;

namespace PLinkageApp
{
    public partial class App : Application
    {
        private readonly Shell _appShell;

        // Parameterless constructor for XAML
        public App()
        {
            InitializeComponent();
        }

        // DI constructor
        public App(Shell appShell) : this()
        {
            _appShell = appShell;
            MainPage = appShell; // Set as MainPage
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_appShell ?? MainPage);
        }
    }
}
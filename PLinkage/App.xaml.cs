using PLinkage.Interfaces;

namespace PLinkage
{
    public partial class App : Application
    {
        private readonly Shell _appShell;

        public App(Shell appShell)
        {
            InitializeComponent();
            _appShell = appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_appShell);
        }
    }

}

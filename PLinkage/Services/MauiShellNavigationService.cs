using PLinkage.Interfaces;

namespace PLinkage.Services
{
    public class MauiShellNavigationService : INavigationService
    {
        private readonly ISessionService _sessionService;

        public MauiShellNavigationService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            if (!route.StartsWith("/") && !route.StartsWith("///"))
            {
                // Assume it's a Shell visual element if declared in XAML
                route = "///" + route;
            }

            if (parameters == null)
                await Shell.Current.GoToAsync(route);
            else
                await Shell.Current.GoToAsync(route, parameters);
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task NavigateToRootAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }   
    }

}

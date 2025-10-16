using PLinkageApp.Interfaces;

namespace PLinkageApp.Services
{
    public class MauiShellNavigationService : INavigationService
    {
        private readonly ISessionService _sessionService;

        public MauiShellNavigationService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        // The reason why we use 3 dash is because the route is instantiated in the shell and in separate
        // To cater to times when a back button is needed, we will need to hardcode it
        // Idea: Remember previous and current page to always know which page to go back to
        // Also remember: previousProject, previousSkillProvider
        public async Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            // 1. If the route is not an absolute path (// or ///), ensure it starts with a single slash
            // to perform normal push navigation within the current Shell hierarchy.
            if (!route.StartsWith("/"))
            {
                // For standard navigation (e.g., from Dashboard to DetailPage),
                // we'll use a single slash. Your current logic uses '///' which
                // always clears the stack. I've adjusted this for standard behavior:
                route = $"/{route}";
            }

            // Note: If you want to support routes like "///LoginPage" directly, 
            // you can allow the user to pass that full route string.

            if (parameters == null)
                await Shell.Current.GoToAsync(route);
            else
                await Shell.Current.GoToAsync(route, parameters);
        }

        public async Task NavigateAndClearStackAsync(string route, IDictionary<string, object>? parameters = null)
        {
            // The '///' prefix ensures that the entire navigation stack is cleared
            // and the target page becomes the new root of the application's history.

            // 1. Clean up the route (remove any leading slashes the caller might have added).
            string absoluteRoute = route.TrimStart('/');

            // 2. Prepend the absolute navigation prefix.
            absoluteRoute = $"///{absoluteRoute}";

            if (parameters == null)
                await Shell.Current.GoToAsync(absoluteRoute);
            else
                await Shell.Current.GoToAsync(absoluteRoute, parameters);
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

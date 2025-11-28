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
            // CASE 1: Backward Navigation
            if (route.StartsWith(".."))
            {
                var navStack = Shell.Current.Navigation.NavigationStack;

                // SAFETY GUARD: 
                // If we are trying to go back 2 steps ("../.."), we need at least 3 pages in the stack.
                // If we are trying to go back 1 step (".."), we need at least 2 pages.

                int stepsBack = route.Split('/').Count(x => x == "..");
                if (navStack.Count <= stepsBack)
                {
                    // Not enough pages to go back that far. 
                    // Optional: Fallback to root or just return to prevent crash.
                    return;
                }

                // Pass the route exactly as is (no slash added)
                if (parameters == null)
                    await Shell.Current.GoToAsync(route);
                else
                    await Shell.Current.GoToAsync(route, parameters);

                return;
            }

            // CASE 2: Forward/Absolute Navigation
            // Only prepend slash if it's not ".." and doesn't already have one
            if (!route.StartsWith("/") && !route.StartsWith("//"))
            {
                route = $"/{route}";
            }

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

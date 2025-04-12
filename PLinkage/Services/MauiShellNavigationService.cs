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

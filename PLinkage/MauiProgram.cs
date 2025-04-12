using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PLinkage.Interfaces;
using PLinkage.Services;
using PLinkage.ViewModels;

namespace PLinkage;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<ISessionService, SessionService>();
		builder.Services.AddTransient<INavigationService, MauiShellNavigationService>();
        

        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddTransient<LoginViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

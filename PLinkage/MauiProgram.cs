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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // Register services and viewmodels for DI
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<AppShellViewModel>();

        // Register your AppShell (so DI works there too)
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

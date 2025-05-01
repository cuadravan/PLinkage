using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PLinkage.Interfaces;
using PLinkage.Services;
using PLinkage.ViewModels;
using PLinkage.Repositories;
using PLinkage.Views;

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
        builder.Services.AddTransient<IAuthenticationService, JsonAuthenticationService>();
		builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
		builder.Services.AddTransient<IStartupService, StartupService>();

        builder.Services.AddTransient<SplashScreenPage>();
        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<LoginView>();


        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ProjectOwnerHomeViewModel>();
		builder.Services.AddTransient<ProjectOwnerProfileViewModel>();
		builder.Services.AddTransient<UpdateProfileViewModel>();
		builder.Services.AddTransient<AddProjectViewModel>();
		builder.Services.AddTransient<ViewProjectViewModel>();
		builder.Services.AddTransient<UpdateProjectViewModel>();
		builder.Services.AddTransient<RateSkillProviderViewModel>();
		builder.Services.AddTransient<BrowseSkillProviderViewModel>();
        builder.Services.AddTransient<SkillProviderProfileViewModel>();
		builder.Services.AddTransient<SendOfferViewModel>();
        builder.Services.AddTransient<ProjectOwnerApplicationOfferViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

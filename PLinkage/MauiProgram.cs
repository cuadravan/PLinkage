using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PLinkage.Interfaces;
using PLinkage.Services;
using PLinkage.ViewModels;
using PLinkage.Repositories;
using PLinkage.Models;

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
        builder.Services.AddTransient<IRepository<Admin>, AdminRepository>();
        builder.Services.AddTransient<IRepository<ProjectOwner>, ProjectOwnerRepository>();
        builder.Services.AddTransient<IRepository<SkillProvider>, SkillProviderRepository>();
        builder.Services.AddTransient<IRepository<Project>, ProjectRepository>();
        builder.Services.AddTransient<IRepository<Message>, MessageRepository>();
        builder.Services.AddTransient<IRepository<OfferApplication>, OfferApplicationRepository>();




        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

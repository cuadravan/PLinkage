using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PLinkageApp.ViewModels;
using PLinkageApp.Views;
using Microsoft.Maui.LifecycleEvents;
using PLinkageApp.Services;
using PLinkageApp.Services.Http;
using PLinkageApp.Interfaces;
using PLinkageApp.Repositories;
using PLinkageApp.ViewsAndroid;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;
#endif

namespace PLinkageApp;

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

        #if ANDROID
            ServiceCollectionServiceExtensions.AddSingleton<Shell, AppShellAndroid>(builder.Services);
        #elif WINDOWS
            ServiceCollectionServiceExtensions.AddSingleton<Shell, AppShellWindows>(builder.Services);
#endif

#if ANDROID
           const string ApiBaseUrl = "http://10.0.2.2:5015/";
#elif WINDOWS
            const string ApiBaseUrl = "http://localhost:5015/";
#else
            const string ApiBaseUrl = "http://192.168.1.8:5030/"; // fallback for other platforms
#endif

        builder.Services.AddHttpClient<BaseApiClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });

        builder.Services.AddHttpClient<IAccountServiceClient, AccountServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });
        builder.Services.AddHttpClient<IDashboardServiceClient, DashboardServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });
        builder.Services.AddHttpClient<IProjectServiceClient, ProjectServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });
        builder.Services.AddHttpClient<IProjectOwnerServiceClient, ProjectOwnerServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });
        builder.Services.AddHttpClient<ISkillProviderServiceClient, SkillProviderServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });
        builder.Services.AddHttpClient<IChatServiceClient, ChatServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });

        builder.Services.AddSingleton<ISessionService, SessionService>();
		builder.Services.AddTransient<INavigationService, MauiShellNavigationService>();
        //builder.Services.AddTransient<IAuthenticationService, JsonAuthenticationService>();
		builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
		builder.Services.AddTransient<IStartupService, StartupService>();

        builder.Services.AddTransient<SplashScreenPage>();
        //builder.Services.AddSingleton<App>();
        //builder.Services.AddSingleton<LogoutView>();            

        //builder.Services.AddTransient<PLinkageApp.ViewsAndroid.SkillProviderHomeView>();
        builder.Services.AddTransient<SkillProviderHomeViewModelTemp>();

        //builder.Services.AddTransient<PLinkageApp.ViewsAndroid.ProjectOwnerHomeView>();
        builder.Services.AddTransient<ProjectOwnerHomeViewModelTemp>();

        //builder.Services.AddTransient<PLinkageApp.ViewsAndroid.AdminHomeView>();
        builder.Services.AddTransient<AdminHomeViewModelTemp>();

        //builder.Services.AddTransient<RegisterView1>();
        //builder.Services.AddTransient<RegisterView2>();
        //builder.Services.AddTransient<RegisterView3>();
        //builder.Services.AddTransient<RegisterView4>();
        //builder.Services.AddTransient<RegisterView5>();
        builder.Services.AddScoped<RegisterViewModelTemp>();

        builder.Services.AddTransient<AdminBrowseSkillProviderViewModelTemp>();
        builder.Services.AddTransient<AdminBrowseProjectViewModelTemp>();
        builder.Services.AddTransient<AdminBrowseProjectOwnerViewModelTemp>();
        builder.Services.AddTransient<ChatViewModelTemp>();
        builder.Services.AddTransient<MessagesViewModelTemp>();

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
        builder.Services.AddTransient<ViewSkillProviderProfileViewModel>();
		builder.Services.AddTransient<SendOfferViewModel>();
        builder.Services.AddTransient<ProjectOwnerApplicationOfferViewModel>();
		builder.Services.AddTransient<SendMessageViewModel>();
        builder.Services.AddTransient<ViewMessagesViewModel>();
        builder.Services.AddTransient<SkillProviderHomeViewModel>();
		builder.Services.AddTransient<SkillProviderProfileViewModel>();
        builder.Services.AddTransient<AddEducationViewModel>();
        builder.Services.AddTransient<UpdateEducationViewModel>();
        builder.Services.AddTransient<AddSkillViewModel>();
        builder.Services.AddTransient<UpdateSkillViewModel>();
		builder.Services.AddTransient<BrowseProjectViewModel>();
        builder.Services.AddTransient<SendApplicationViewModel>();
        builder.Services.AddTransient<ViewProjectOwnerProfileViewModel>();
        builder.Services.AddTransient<SkillProviderApplicationOfferViewModel>();
        builder.Services.AddTransient<AdminHomeViewModel>();
        builder.Services.AddTransient<AdminBrowseProjectsViewModel>();
        builder.Services.AddTransient<AdminBrowseSkillProviderViewModel>();
        builder.Services.AddTransient<AdminBrowseProjectOwnerViewModel>();

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            // Add Windows-specific lifecycle events
            events.AddWindows(wndLifeCycleBuilder =>
            {
                // This runs when a window is created
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    // Get the window handle
                    nint hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                    // Get the Window ID from the handle
                    WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);

                    // Get the AppWindow instance
                    AppWindow appWindow = AppWindow.GetFromWindowId(myWndId);

                    // Set the presenter to FullScreen
                    appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                });
            });
        });
#endif


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

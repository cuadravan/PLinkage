using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PLinkageApp.ViewModels;
using Microsoft.Maui.LifecycleEvents;
using PLinkageApp.Services;
using PLinkageApp.Services.Http;
using PLinkageApp.Interfaces;

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
        // CHANGE THIS FROM AddSingleton TO AddTransient
        ServiceCollectionServiceExtensions.AddTransient<Shell, AppShellAndroid>(builder.Services);
#elif WINDOWS
            // CHANGE THIS FROM AddSingleton TO AddTransient
            ServiceCollectionServiceExtensions.AddTransient<Shell, AppShellWindows>(builder.Services);
#endif

        //#if ANDROID
        //        const string ApiBaseUrl = "http://10.0.2.2:5015/";
        //#elif WINDOWS
        //            const string ApiBaseUrl = "http://localhost:5015/";
        //#else
        //            const string ApiBaseUrl = "http://192.168.1.8:5030/"; // fallback for other platforms
        //#endif
        const string ApiBaseUrl = "https://plinkageapi-hzddfah5gbhcfxdm.southeastasia-01.azurewebsites.net/";

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
        builder.Services.AddHttpClient<IOfferApplicationServiceClient, OfferApplicationServiceClient>(client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
        });

        builder.Services.AddSingleton<ISessionService, SessionService>();
		builder.Services.AddSingleton<INavigationService, MauiShellNavigationService>();       
		builder.Services.AddSingleton<IStartupService, StartupService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<SplashScreenPage>();
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<RegisterViewModel>();

        builder.Services.AddTransient<ChatViewModel>();
        builder.Services.AddTransient<MessagesViewModel>();
        builder.Services.AddTransient<ViewSkillViewModel>();
        builder.Services.AddTransient<ResignProjectViewModel>();
        builder.Services.AddTransient<NegotiateViewModel>();
        builder.Services.AddTransient<ProcessResignationViewModel>();
        builder.Services.AddTransient<ProjectOwnerLinkagesViewModel>();
        builder.Services.AddTransient<SkillProviderLinkagesViewModel>();
        builder.Services.AddTransient<LoginViewModel>();       
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
        builder.Services.AddTransient<SkillProviderHomeViewModel>();
		builder.Services.AddTransient<SkillProviderProfileViewModel>();
        builder.Services.AddTransient<AddEducationViewModel>();
        builder.Services.AddTransient<UpdateEducationViewModel>();
        builder.Services.AddTransient<AddSkillViewModel>();
		builder.Services.AddTransient<BrowseProjectViewModel>();
        builder.Services.AddTransient<SendApplicationViewModel>();
        builder.Services.AddTransient<ViewProjectOwnerProfileViewModel>();
        builder.Services.AddTransient<AdminHomeViewModel>();
        builder.Services.AddTransient<AdminBrowseProjectOwnerViewModel>();

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    // 1. Get the window handle
                    nint hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                    // 2. Get the Window ID from the handle
                    Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);

                    // 3. Get the AppWindow instance
                    Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);

                    // 4. MAXIMIZE (Instead of FullScreen)
                    // We check if the current presenter is an "OverlappedPresenter" (standard window)
                    // and then call Maximize on it.
                    if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
                    {
                        p.Maximize();
                    }
                    else
                    {
                        // Fallback: Force it to be Overlapped, then Maximize
                        appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);
                        if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p2)
                        {
                            p2.Maximize();
                        }
                    }
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

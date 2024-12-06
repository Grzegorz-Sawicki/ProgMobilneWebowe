using L5Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using L5MAUI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using L5MAUI.ViewModels;
using L5Shared.MessageBox;
using L5MAUI.MessageBox;
using System.Net.Http.Headers;
using L5MAUI.Pages;

namespace L5MAUI
{
    public partial class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                // Adjust the base URL for each platform
#if ANDROID
                client.BaseAddress = new Uri("http://10.0.2.2:5174/api/"); // Android Emulator
#elif IOS
            client.BaseAddress = new Uri("http://127.0.0.1:5174/api/"); // iOS Simulator
#else
            client.BaseAddress = new Uri("http://localhost:5174/api/"); // Windows or MacCatalyst
#endif
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            ConfigureServices(builder.Services);
            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            ConfigureAppServices(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
        }

        private static void ConfigureAppServices(IServiceCollection services)
        {
            services.AddTransient<IActorService, ActorService>();
            services.AddTransient<IDirectorService, DirectorService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();
        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            services.AddSingleton<MoviesViewModel>();
            services.AddSingleton<AddMovieViewModel>();
            services.AddSingleton<AddActorViewModel>();
            services.AddSingleton<AddDirectorViewModel>();
        }

        private static void ConfigureViews(IServiceCollection services)
        {
            services.AddSingleton<MainPage>();
            services.AddTransient<AddMovieView>();
            services.AddTransient<AddActorPage>();
            services.AddTransient<AddDirectorPage>();
        }


    }
}

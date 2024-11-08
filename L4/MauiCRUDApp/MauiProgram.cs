using CRUDAppService.Confguration;
using CRUDAppService.MessageBox;
using CRUDAppService.Services.ProductService;
using MAUIAppCRUD.MessageBox;
using Microsoft.Extensions.Logging;
using MAUIAppCRUD.ViewModels;
namespace MAUIAppCRUD
{
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            ConfigureServices(builder.Services);
            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var appSettings = ConfigureAppSettings(services);
            ConfigureAppServices(services, appSettings);
            ConfigureViewModels(services);
            ConfigureViews(services);
        }

        private static void ConfigureAppServices(IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();

        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            // tutaj konfigurujemy viewmodele

            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProductDetailsViewModel>();
        }

        private static void ConfigureViews(IServiceCollection services)
        {
            // tutaj konfigurujemy widoki
            services.AddSingleton<MainPage>();
            services.AddTransient<ProductDetailsView>();
        }

        private static AppSettings ConfigureAppSettings(IServiceCollection services)
        {
            //pobranie ustawień z pliku konfiguracyjnego
            // i zmapowanie ich na obiekt AppSettings
            //potrzebujemy pakietu Microsoft.Extensions.Options.ConfigurationExtensions

            //var appSettingsSection = _configuration.GetSection(nameof(AppSettings));
            //var settings = appSettingsSection.Get<AppSettings>();
            //services.Configure<AppSettings>(appSettingsSection); // zarejestrowanie AppSettings w kontenerze DI
            //return settings;

            var appSettingsSection = new AppSettings()
            {
                BaseApiUrl = "http://localhost:5237",
                ProductEndpoint = new ProductEndpoint()
                {
                    BaseUrl = "api/product/",
                    GetProducts = "",
                    CreateProduct = "",
                    UpdateProduct = "",
                    SearchProducts = "",
                }
            };

            services.Configure<AppSettings>(options =>
            {
                options.BaseApiUrl = appSettingsSection.BaseApiUrl;
                options.ProductEndpoint = appSettingsSection.ProductEndpoint;
            });
            return appSettingsSection;



        }
    }
}

using CRUDAppService.MessageBox;
using CRUDAppService.Services.ProductService;
using MAUIAppCRUD.MessageBox;
using Microsoft.Extensions.Logging;
using MAUIAppCRUD.ViewModels;
using CRUDAppService.Services;
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
            ConfigureAppServices(services);
            ConfigureViewModels(services);
            ConfigureViews(services);
        }

        private static void ConfigureAppServices(IServiceCollection services)
        {
            //konfiguracja DI
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IMessageDialogService, MauiMessageDialogService>();
            services.AddSingleton<IFileService, FileService>();

        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            // konfiguracja ViewModel
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProductDetailsViewModel>();
        }

        private static void ConfigureViews(IServiceCollection services)
        {
            // konfiguracja View
            services.AddSingleton<MainPage>();
            services.AddTransient<ProductDetailsView>();
        }
    }
}

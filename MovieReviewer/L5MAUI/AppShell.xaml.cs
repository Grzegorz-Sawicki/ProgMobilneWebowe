using L5MAUI.Pages;

namespace L5MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddMovieView), typeof(AddMovieView));
            Routing.RegisterRoute(nameof(AddActorPage), typeof(AddActorPage));
            Routing.RegisterRoute(nameof(AddDirectorPage), typeof(AddDirectorPage));
        }
    }
}

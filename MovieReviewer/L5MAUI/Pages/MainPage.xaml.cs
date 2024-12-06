using L5MAUI.ViewModels;

namespace L5MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MoviesViewModel moviesViewModel)
        {
            BindingContext = moviesViewModel;
            InitializeComponent();
        }

    }

}

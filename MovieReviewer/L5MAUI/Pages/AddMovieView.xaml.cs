using L5MAUI.ViewModels;

namespace L5MAUI.Pages;

public partial class AddMovieView : ContentPage
{
	public AddMovieView(AddMovieViewModel addMovieViewModel)
	{
		BindingContext = addMovieViewModel;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		if (BindingContext is AddMovieViewModel viewModel)
		{
			viewModel.OnNavigatedTo();
		}
    }
}
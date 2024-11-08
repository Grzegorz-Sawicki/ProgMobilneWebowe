using MauiCRUDApp.ViewModels;

namespace MauiCRUDApp;

public partial class MainPage : ContentPage
{
	public MainPage(ProductsViewModel productsViewModel)
	{
		BindingContext = productsViewModel;
		InitializeComponent();
	}


}


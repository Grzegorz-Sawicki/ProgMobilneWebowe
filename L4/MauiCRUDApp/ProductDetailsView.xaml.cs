using MauiCRUDApp.ViewModels;

namespace MauiCRUDApp;

public partial class ProductDetailsView : ContentPage
{
	public ProductDetailsView(ProductDetailsViewModel productDetailsViewModel)
	{
		BindingContext = productDetailsViewModel;
		InitializeComponent();
	}
}
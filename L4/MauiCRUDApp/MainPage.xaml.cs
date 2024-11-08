using MAUIAppCRUD.ViewModels;

namespace MAUIAppCRUD;

public partial class MainPage : ContentPage
{
	public MainPage(ProductsViewModel productsViewModel)
	{
		BindingContext = productsViewModel;
		InitializeComponent();
	}


}


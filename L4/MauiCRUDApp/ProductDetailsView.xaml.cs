using MAUIAppCRUD.ViewModels;

namespace MAUIAppCRUD;

public partial class ProductDetailsView : ContentPage
{
	public ProductDetailsView(ProductDetailsViewModel productDetailsViewModel)
	{
		BindingContext = productDetailsViewModel;
		InitializeComponent();
	}
}
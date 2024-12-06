using L5MAUI.ViewModels;

namespace L5MAUI.Pages;

public partial class AddDirectorPage : ContentPage
{
	public AddDirectorPage(AddDirectorViewModel addDirectorViewModel)
	{
		BindingContext = addDirectorViewModel;
		InitializeComponent();
	}
}
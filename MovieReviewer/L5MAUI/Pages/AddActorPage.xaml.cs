using L5MAUI.ViewModels;

namespace L5MAUI.Pages;

public partial class AddActorPage : ContentPage
{
	public AddActorPage(AddActorViewModel addActorViewModel)
	{
		BindingContext = addActorViewModel;
		InitializeComponent();
	}
}
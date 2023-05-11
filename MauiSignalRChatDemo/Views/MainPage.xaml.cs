using MauiSignalRChatDemo.ViewModels;

namespace MauiSignalRChatDemo;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}


using System.ComponentModel;
using OQF.PlayerVsBot.ViewModels.MainWindow;

namespace OQF.PlayerVsBot.Windows
{
	public partial class MainWindow
	{
		public MainWindow ()
		{
			InitializeComponent();
		}

		private async void MainWindow_OnClosing (object sender, CancelEventArgs e)
		{
			var viewModel = DataContext as IMainWindowViewModel;

			if (viewModel.PreventWindowClosingToAskUser)
			{
				e.Cancel = true;


//				var winningDialogViewModel = new WinningDialogViewModel();
//
//				var view = new Views.WinningDialog
//				{
//					DataContext = winningDialogViewModel
//				};
//
//				var dialogResult = await DialogHost.Show(view, "RootDialog");
//
//				if ((bool)dialogResult)
//				{
//					
//				}
//
//				winningDialogViewModel.Dispose();


				//				var dialog = new UserDialogBox("Programm beenden.", "Wollen Sie das Programm wirklich beenden?",
				//				MessageBoxButton.OKCancel);
				//
				//				var result = await dialog.ShowMahAppsDialog();
				//
				//				if (result == MessageDialogResult.Affirmative)
				//				{
				//					viewModel.CloseWindow.Execute(null);
				//				}
			}
		}
	}
}

using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using OQF.PlayerVsBot.ViewModels.MainWindow;
using OQF.PlayerVsBot.ViewModels.YesNoDialog;

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
			var mainWindowViewModel = DataContext as IMainWindowViewModel;

			if (mainWindowViewModel.PreventWindowClosingToAskUser)
			{
				e.Cancel = true;

				var closingDialogViewModel = new YesNoDialogViewModel("schließen?");
				var closingDialog = new Views.YesNoDialog
				{
					DataContext = closingDialogViewModel
				};

				var closingDialogResult = await DialogHost.Show(closingDialog, "RootDialog");

				if ((bool) closingDialogResult)
				{
					var savingDialogViewModel = new YesNoDialogViewModel("speichern?");
					var savingDialog = new Views.YesNoDialog
					{
						DataContext = savingDialogViewModel
					};

					var savingDialogResult = await DialogHost.Show(savingDialog, "RootDialog");

					if ((bool) savingDialogResult)
					{
						if (mainWindowViewModel.DumpProgressToFile.CanExecute(null))
							mainWindowViewModel.DumpProgressToFile.Execute(null);
					}

					mainWindowViewModel.CloseWindow.Execute(null);
				}
				
				closingDialogViewModel.Dispose();				
			}
		}
	}
}

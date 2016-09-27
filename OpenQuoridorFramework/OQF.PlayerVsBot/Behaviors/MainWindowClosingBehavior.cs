using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace OQF.PlayerVsBot.Behaviors
{
	internal class MainWindowClosingBehavior: Behavior<Window>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Closing += OnClosing;
		}		

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.Closing -= OnClosing;
		}

		private async void OnClosing (object sender, CancelEventArgs cancelEventArgs)
		{
#if !DEBUG
			var mainWindowViewModel = AssociatedObject.DataContext as IMainWindowViewModel;

			if (mainWindowViewModel.PreventWindowClosingToAskUser)
			{
				cancelEventArgs.Cancel = true;

				var closingDialogViewModel = new YesNoDialogViewModel(Captions.ClosingDialogMessage);
				var closingDialog = new Views.YesNoDialog
				{
					DataContext = closingDialogViewModel
				};

				var closingDialogResult = await DialogHost.Show(closingDialog, "RootDialog");

				if ((bool)closingDialogResult)
				{
					var savingDialogViewModel = new YesNoDialogViewModel(Captions.SavingDialogMessage);
					var savingDialog = new Views.YesNoDialog
					{
						DataContext = savingDialogViewModel
					};

					var savingDialogResult = await DialogHost.Show(savingDialog, "RootDialog");

					if ((bool)savingDialogResult)
					{
						if (mainWindowViewModel.DumpProgressToFile.CanExecute(null))
							mainWindowViewModel.DumpProgressToFile.Execute(null);
					}

					mainWindowViewModel.CloseWindow.Execute(null);
				}

				closingDialogViewModel.Dispose();

			}
#endif
		}
	}
}

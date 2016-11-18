using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using OQF.CommonUiElements.Dialogs.YesNo;
using OQF.PlayerVsBot.Visualization.ViewModels.MainWindow;
using OQF.Resources.LanguageDictionaries;

namespace OQF.PlayerVsBot.Visualization.Behaviors
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
			var mainWindowViewModel = AssociatedObject.DataContext as IMainWindowViewModel;

			if (mainWindowViewModel.PreventWindowClosingToAskUser)
			{
				cancelEventArgs.Cancel = true;
				
				var closingDialogResult = await YesNoDialogService.Show(Captions.ClosingDialogMessage);

				if (closingDialogResult)
				{					
					var savingDialogResult = await YesNoDialogService.Show(Captions.SavingDialogMessage);

					if (savingDialogResult)
					{
						if (mainWindowViewModel.ProgressViewModel.DumpProgressToFile.CanExecute(null))
							mainWindowViewModel.ProgressViewModel.DumpProgressToFile.Execute(null);
					}

					mainWindowViewModel.CloseWindow.Execute(null);
				}				
			}
		}
	}
}

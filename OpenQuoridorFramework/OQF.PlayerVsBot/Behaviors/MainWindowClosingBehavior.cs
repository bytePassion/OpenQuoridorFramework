﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using MaterialDesignThemes.Wpf;
using OQF.PlayerVsBot.ViewModels.MainWindow;
using OQF.Visualization.Common.Dialogs.YesNo;
using OQF.Visualization.Common.Dialogs.YesNo.ViewModel;
using OQF.Visualization.Resources.LanguageDictionaries;

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
			var mainWindowViewModel = AssociatedObject.DataContext as IMainWindowViewModel;

			if (mainWindowViewModel.PreventWindowClosingToAskUser)
			{
				cancelEventArgs.Cancel = true;

				var closingDialogViewModel = new YesNoDialogViewModel(Captions.ClosingDialogMessage);
				var closingDialog = new YesNoDialog
				{
					DataContext = closingDialogViewModel
				};

				var closingDialogResult = await DialogHost.Show(closingDialog, "RootDialog");

				if ((bool)closingDialogResult)
				{
					var savingDialogViewModel = new YesNoDialogViewModel(Captions.SavingDialogMessage);
					var savingDialog = new YesNoDialog
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
		}
	}
}

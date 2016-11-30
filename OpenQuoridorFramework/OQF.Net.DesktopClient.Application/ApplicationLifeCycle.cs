﻿using System.Windows;
using Lib.Communication.State;
using Lib.Wpf;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardHorizontalLabeling;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private INetworkGameService networkGameService;

		public void BuildAndStart (StartupEventArgs startupEventArgs)
		{
			var isBoardRotatedVariable = new SharedState<bool>(false);

			networkGameService = new NetworkGameService(isBoardRotatedVariable);

			var boardPlacementViewModel = new BoardPlacementViewModel(networkGameService);
			var boardViewModel = new BoardViewModel(networkGameService);
			var progressViewModel = new ProgressViewModel(networkGameService);
			var languageSelectionViewModel = new LanguageSelectionViewModel();
			var actionBarViewModel = new ActionBarViewModel(languageSelectionViewModel, networkGameService);
			var boardHorizontalLabelingViewModel = new BoardHorizontalLabelingViewModel(isBoardRotatedVariable);
			var boardVerticalLabelingViewModel   = new BoardVerticalLabalingViewModel(isBoardRotatedVariable);

			var mainWindowViewModel = new MainWindowViewModel(networkGameService, 
															  isBoardRotatedVariable,
															  boardPlacementViewModel, 
															  boardViewModel, 
															  progressViewModel,
															  actionBarViewModel,
															  boardHorizontalLabelingViewModel,
															  boardVerticalLabelingViewModel);

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp (ExitEventArgs exitEventArgs)
		{
			networkGameService.Dissconnect();
		}
	}
}

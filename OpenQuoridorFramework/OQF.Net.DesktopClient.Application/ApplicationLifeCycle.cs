﻿using System.Globalization;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Utils;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.CommonUiElements.ProgressView.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Net.DesktopClient.NetworkGameLogic;
using OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar;
using OQF.Net.DesktopClient.Visualization.ViewModels.MainWindow;
using OQF.Net.DesktopClient.Visualization.ViewModels.NetworkView;
using OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar;
using OQF.Utils;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationLifeCycle : IApplicationLifeCycle
	{
		private INetworkGameService networkGameService;

		public void BuildAndStart (StartupEventArgs startupEventArgs)
		{
			var isBoardRotatedVariable = new SharedState<bool>(false);

			networkGameService = new NetworkGameService(isBoardRotatedVariable);
			IApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();

			CultureManager.CurrentCulture = new CultureInfo(applicationSettingsRepository.SelectedLanguageCode);
			CultureManager.CultureChanged += () =>
			{
				applicationSettingsRepository.SelectedLanguageCode = CultureManager.CurrentCulture.ToString();
			};

			var boardPlacementViewModel          = new BoardPlacementViewModel(networkGameService);
			var boardViewModel                   = new BoardViewModel(networkGameService);
			var progressViewModel                = new ProgressViewModel(networkGameService);
			var languageSelectionViewModel       = new LanguageSelectionViewModel();
			var actionBarViewModel               = new ActionBarViewModel(languageSelectionViewModel, networkGameService);
			var boardHorizontalLabelingViewModel = new BoardHorizontalLabelingViewModel(isBoardRotatedVariable);
			var boardVerticalLabelingViewModel   = new BoardVerticalLabalingViewModel(isBoardRotatedVariable);
			var localPlayerBarViewModel          = new LocalPlayerBarViewModel(networkGameService);
			var remotePlayerBarViewModel         = new RemotePlayerBarViewModel(networkGameService);
			var networkViewModel                 = new NetworkViewModel(networkGameService, applicationSettingsRepository);

			var mainWindowViewModel = new MainWindowViewModel(networkGameService,
															  isBoardRotatedVariable,
															  boardPlacementViewModel, 
															  boardViewModel, 
															  progressViewModel,
															  actionBarViewModel,
															  boardHorizontalLabelingViewModel,
															  boardVerticalLabelingViewModel,
															  localPlayerBarViewModel,
															  remotePlayerBarViewModel, 
															  networkViewModel);

			var mainWindow = new Visualization.Windows.MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}

		public void CleanUp (ExitEventArgs exitEventArgs)
		{
			networkGameService.Disconnect();
		}
	}
}

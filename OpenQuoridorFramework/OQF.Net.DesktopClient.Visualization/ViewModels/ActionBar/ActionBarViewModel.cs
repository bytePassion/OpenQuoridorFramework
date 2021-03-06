﻿using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.CommonUiElements.Info;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.Net.DesktopClient.Contracts;
using OQF.Resources;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar
{
	public class ActionBarViewModel : ViewModel, IActionBarViewModel
	{
		private readonly INetworkGameService networkGameService;
		private string initiatorPlayerName;
		private string opponentPlayerName;
		private string gameName;

		public ActionBarViewModel(ILanguageSelectionViewModel languageSelectionViewModel, INetworkGameService networkGameService)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			LanguageSelectionViewModel = languageSelectionViewModel;
			this.networkGameService = networkGameService;
			
			networkGameService.GameStatusChanged += OnGameStatusChanged;

			ShowAboutHelp = new Command(DoShowAboutHelp);

			OnGameStatusChanged(networkGameService.CurrentGameStatus);
		}		

		private void OnGameStatusChanged(GameStatus newGameStatus)
		{
			switch (newGameStatus)
			{
				case GameStatus.PlayingJoinedGame:
				{
					InitiatorPlayerName = networkGameService.OpponendPlayer.Name;
					OpponentPlayerName  = networkGameService.ClientPlayer.Name;
					GameName = networkGameService.GameName + ":";
					break;
				}
				case GameStatus.PlayingOpendGame:
				{
					InitiatorPlayerName = networkGameService.ClientPlayer.Name;
					OpponentPlayerName  = networkGameService.OpponendPlayer.Name; 
					GameName = networkGameService.GameName + ":";
					break;
				}
				default:
				{
					InitiatorPlayerName = "-----";
					OpponentPlayerName = "-----";
					GameName = Captions.NCl_NoGameStartedNotice;
					break;
				}
			}
		}
		
		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand ShowAboutHelp { get; }

		public string InitiatorPlayerName
		{
			get { return initiatorPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref initiatorPlayerName, value); }
		}

		public string OpponentPlayerName
		{
			get { return opponentPlayerName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref opponentPlayerName, value); }
		}

		public string GameName
		{
			get { return gameName; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gameName, value); }
		}


		private void DoShowAboutHelp ()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.NetworkDesktopClient.Info,
								   //InfoPage.DesktopClieApplicationInfo,
								   InfoPage.QuoridorRules,
								   InfoPage.QuoridorNotation,								   
								   InfoPage.About);
		}
		
		public string OpenInfoButtonToolTipCaption => Captions.PvB_OpenInfoButtonToolTipCaption;
		
		private void RefreshCaptions ()
		{
			if (networkGameService.CurrentGameStatus != GameStatus.PlayingJoinedGame &&
			    networkGameService.CurrentGameStatus != GameStatus.PlayingOpendGame)
			{
				GameName = Captions.NCl_NoGameStartedNotice;
			}

			PropertyChanged.Notify(this, nameof(OpenInfoButtonToolTipCaption));			
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;

			networkGameService.GameStatusChanged -= OnGameStatusChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

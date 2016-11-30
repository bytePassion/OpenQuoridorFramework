using System.ComponentModel;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
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
			
			networkGameService.JoinSuccessful      += OnJoinSuccessful;
			networkGameService.OpendGameIsStarting += OnOpendGameIsStarting;

			ShowAboutHelp = new Command(DoShowAboutHelp);

			InitiatorPlayerName = "-----";
			OpponentPlayerName = "-----";
			GameName = "no game started";
		}

		private void OnOpendGameIsStarting(string s)
		{
			InitiatorPlayerName = networkGameService.PlayerName;
			OpponentPlayerName = s;
			GameName = networkGameService.GameName;
		}

		private void OnJoinSuccessful(string s)
		{
			InitiatorPlayerName = s;
			OpponentPlayerName = networkGameService.PlayerName;
			GameName = networkGameService.GameName;
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
			PropertyChanged.Notify(this, nameof(OpenInfoButtonToolTipCaption));			
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
			
			networkGameService.JoinSuccessful      -= OnJoinSuccessful;
			networkGameService.OpendGameIsStarting -= OnOpendGameIsStarting;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

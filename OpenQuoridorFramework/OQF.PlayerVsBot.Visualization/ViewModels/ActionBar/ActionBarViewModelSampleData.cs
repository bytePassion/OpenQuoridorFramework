using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OQF.Bot.Contracts.Coordination;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar.Helper;

#pragma warning disable 0067

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar
{
	internal class ActionBarViewModelSampleData : IActionBarViewModel
	{
		public ActionBarViewModelSampleData()
		{
			LanguageSelectionViewModel = new LanguageSelectionViewModelSampleData();

			DllPathInput = "blubb.dll";			
			IsStartWithProgressPopupVisible = true;
			TopPlayerName = "PlayerOben";

			StartOptions = new List<StartOptionsDisplayData>
			{
				new StartOptionsDisplayData(PlayerType.TopPlayer,    "start as TopPlayer"),
				new StartOptionsDisplayData(PlayerType.BottomPlayer, "start as BottomPlayer")
			};
			SelectedOption = StartOptions.First();

			HeaderCaptionPlayer                       = "Spieler";
			BrowseForBotButtonToolTipCaption          = "bot dll laden";
			StartGameButtonToolTipCaption             = "Start";
			StartWithProgressGameButtonToolTipCaption = "Start";
			OpenInfoButtonToolTipCaption              = "Info";
			StartGameFromStringButtonCaption          = "von Zeichenkette";
			StartGameFromFileButtonCaption            = "von Datei";			
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start                       => null;
		public ICommand StartWithProgress           => null;
		public ICommand StartWithProgressFromFile   => null;
		public ICommand StartWithProgressFromString => null;
		public ICommand ShowAboutHelp               => null;
		public ICommand BrowseDll                   => null;

		public IEnumerable<StartOptionsDisplayData> StartOptions { get; }
		public StartOptionsDisplayData SelectedOption { get; set; }

		public string DllPathInput { get; set; }
		public bool IsStartWithProgressPopupVisible { get; set; }

		public string TopPlayerName { get; }

		public string HeaderCaptionPlayer                       { get; }
		public string BrowseForBotButtonToolTipCaption          { get; }
		public string StartGameButtonToolTipCaption             { get; }
		public string StartWithProgressGameButtonToolTipCaption { get; }
		public string OpenInfoButtonToolTipCaption              { get; }
		public string StartGameFromStringButtonCaption          { get; }
		public string StartGameFromFileButtonCaption            { get; }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
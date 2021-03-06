﻿using System.ComponentModel;
using System.Windows.Input;
using OQF.Bot.Contracts.Coordination;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

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

			HeaderCaptionPlayer                       = "Spieler";
			BrowseForBotButtonToolTipCaption          = "bot dll laden";
			StartGameButtonToolTipCaption             = "Start";
			StartWithProgressGameButtonToolTipCaption = "Start";
			OpenInfoButtonToolTipCaption              = "Info";
			StartGameFromStringButtonCaption          = "von Zeichenkette";
			StartGameFromFileButtonCaption            = "von Datei";
			StartOptionBottomPlayer                   = "bottom";
			StartOptionTopPlayer                      = "top";
			StartOptionHeader                         = "Startpostion";

			StartPosition = PlayerType.BottomPlayer;
		}


		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand Start                       => null;
		public ICommand StartWithProgress           => null;
		public ICommand StartWithProgressFromFile   => null;
		public ICommand StartWithProgressFromString => null;
		public ICommand ShowAboutHelp               => null;
		public ICommand BrowseDll                   => null;

		public PlayerType StartPosition { get; set; }

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
		public string StartOptionBottomPlayer                   { get; }
		public string StartOptionTopPlayer                      { get; }
		public string StartOptionHeader                         { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
using System.Collections.Generic;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.PlayerVsBot.Visualization.ViewModels.ActionBar.Helper;

namespace OQF.PlayerVsBot.Visualization.ViewModels.ActionBar
{
	public interface IActionBarViewModel : IViewModel
	{
		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand Start                       { get; }	
		ICommand StartWithProgress           { get; }	
		ICommand StartWithProgressFromFile   { get; }	
		ICommand StartWithProgressFromString { get; }	
		ICommand ShowAboutHelp               { get; }
		ICommand BrowseDll                   { get; }

		IEnumerable<StartOptionsDisplayData> StartOptions { get; }
		StartOptionsDisplayData SelectedOption { get; set; }

		string DllPathInput { get; set; }

		bool IsStartWithProgressPopupVisible { get; set; }

		string TopPlayerName { get; }

		string HeaderCaptionPlayer                       { get; }
		string BrowseForBotButtonToolTipCaption          { get; }
		string StartGameButtonToolTipCaption             { get; }
		string StartWithProgressGameButtonToolTipCaption { get; }
		string OpenInfoButtonToolTipCaption              { get; }
		string StartGameFromStringButtonCaption          { get; }
		string StartGameFromFileButtonCaption            { get; }		
		string StartOptionBottomPlayer                   { get; }
		string StartOptionTopPlayer                      { get; }
		string StartOptionHeader                         { get; }
	}
}
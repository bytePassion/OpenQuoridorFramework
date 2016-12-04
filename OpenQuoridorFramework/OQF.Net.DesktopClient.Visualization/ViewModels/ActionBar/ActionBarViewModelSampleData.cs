using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar
{
	internal class ActionBarViewModelSampleData : IActionBarViewModel
	{

		public ActionBarViewModelSampleData()
		{
			LanguageSelectionViewModel = new LanguageSelectionViewModelSampleData();

			InitiatorPlayerName = "xelor";
			OpponentPlayerName = "alexomanie";
			GameName = "x2_Finals";

			OpenInfoButtonToolTipCaption = "Help";
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand ShowAboutHelp => null;

		public string InitiatorPlayerName { get; }
		public string OpponentPlayerName { get; }
		public string GameName { get; }
		public string OpenInfoButtonToolTipCaption { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}
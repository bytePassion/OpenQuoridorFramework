using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.ActionBar
{
	internal class ActionBarViewModelSampleData : IActionBarViewModel
	{
		public ActionBarViewModelSampleData()
		{
			LanguageSelectionViewModel = new LanguageSelectionViewModelSampleData();

			OpenInfoButtonToolTipCaption = "Info";
		}

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand ShowAboutHelp => null;

		public string OpenInfoButtonToolTipCaption { get; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void Dispose() { }		
	}
}
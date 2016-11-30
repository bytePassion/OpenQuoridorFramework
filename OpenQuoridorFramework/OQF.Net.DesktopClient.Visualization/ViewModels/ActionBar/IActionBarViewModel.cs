using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.ActionBar
{
	public interface IActionBarViewModel : IViewModel
	{
		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand ShowAboutHelp { get; }

		string InitiatorPlayerName { get; }
		string OpponentPlayerName  { get; }
		string GameName { get; }

		string OpenInfoButtonToolTipCaption { get; }
	}
}
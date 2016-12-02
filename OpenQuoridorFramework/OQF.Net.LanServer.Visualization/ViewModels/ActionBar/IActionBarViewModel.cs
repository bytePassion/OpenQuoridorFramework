using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

namespace OQF.Net.LanServer.Visualization.ViewModels.ActionBar
{
	public interface IActionBarViewModel : IViewModel
	{
		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand ShowAboutHelp { get; }
		
		string OpenInfoButtonToolTipCaption { get; }
	}
}
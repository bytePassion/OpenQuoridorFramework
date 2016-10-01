using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
	{

		ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		ICommand CloseWindow { get; }
		string CloseButtonCaption { get; }
	}
}
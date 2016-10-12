using System.Collections.Generic;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Info.Pages.PageViewModels;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;

namespace OQF.CommonUiElements.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
    {
        ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		IEnumerable<IPage> Pages { get; }

        ICommand CloseWindow { get; }
        string CloseButtonCaption { get; }
    }
}
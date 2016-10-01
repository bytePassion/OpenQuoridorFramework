using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.Visualization.Common.Info.Pages.PageViewModels;
using OQF.Visualization.Common.Language.LanguageSelection.ViewModel;

namespace OQF.Visualization.Common.Info.InfoWindow.ViewModel
{
    internal interface IInfoWindowViewModel : IViewModel
    {
        ILanguageSelectionViewModel LanguageSelectionViewModel { get; }
        ObservableCollection<IPage> Pages { get; }
        ICommand CloseWindow { get; }
        string CloseButtonCaption { get; }
    }
}
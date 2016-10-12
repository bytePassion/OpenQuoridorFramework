using System.Collections.Generic;
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Info.Pages.PageViewModels;

namespace OQF.CommonUiElements.Info.InfoWindow.ViewModel
{
	internal interface IInfoWindowViewModel : IViewModel
    {
        IEnumerable<IPage> Pages { get; }

        ICommand CloseWindow { get; }
        string CloseButtonCaption { get; }
    }
}
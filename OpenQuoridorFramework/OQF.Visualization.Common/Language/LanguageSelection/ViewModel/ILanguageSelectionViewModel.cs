using System.Collections.ObjectModel;
using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Language.LanguageSelection.ViewModel
{
	public interface ILanguageSelectionViewModel : IViewModel
	{
		ObservableCollection<string> AvailableCountryCodes { get; }
		string SelectedCountryCode { get; set; }
	}
}
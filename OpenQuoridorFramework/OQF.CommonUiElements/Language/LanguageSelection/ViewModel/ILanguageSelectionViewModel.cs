using System.Collections.ObjectModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace OQF.CommonUiElements.Language.LanguageSelection.ViewModel
{
	public interface ILanguageSelectionViewModel : IViewModel
	{
		ObservableCollection<string> AvailableCountryCodes { get; }
		string SelectedCountryCode { get; set; }
	}
}
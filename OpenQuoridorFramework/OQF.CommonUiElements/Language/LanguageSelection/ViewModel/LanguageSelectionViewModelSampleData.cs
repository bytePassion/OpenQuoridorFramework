using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Language.LanguageSelection.ViewModel
{
	public class LanguageSelectionViewModelSampleData : ILanguageSelectionViewModel
	{
		public LanguageSelectionViewModelSampleData()
		{
			AvailableCountryCodes = new ObservableCollection<string>
			{
				"de",
				"en"
			};

			SelectedCountryCode = AvailableCountryCodes.First();
		}

		public ObservableCollection<string> AvailableCountryCodes { get; }
		public string SelectedCountryCode { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
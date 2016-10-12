using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Lib.FrameworkExtension;
using OQF.Resources;
using OQF.Utils;

namespace OQF.CommonUiElements.Language.LanguageSelection.ViewModel
{
	public class LanguageSelectionViewModel : Lib.Wpf.ViewModelBase.ViewModel, ILanguageSelectionViewModel 
	{
		private string selectedCountryCode;

		public LanguageSelectionViewModel()
		{
			CultureManager.CultureChanged += OnCultureChanged;

			AvailableCountryCodes = new ObservableCollection<string>(OpenQuoridorFrameworkInfo.AvailableLanguageCodes);

			OnCultureChanged();
		}

		private void OnCultureChanged()
		{
			var newCultureCode = CultureManager.CurrentCulture.ToString();

			if (newCultureCode != SelectedCountryCode)
			{
				var culture = AvailableCountryCodes.First(cCode => cCode == newCultureCode);

				if (culture == null)
					throw new Exception();

				SelectedCountryCode = culture;
			}
		}

		public ObservableCollection<string> AvailableCountryCodes { get; }

		public string SelectedCountryCode
		{
			get { return selectedCountryCode; }
			set
			{
				if (value != selectedCountryCode)
				{
					CultureManager.CurrentCulture = new CultureInfo(value);					
				}
				PropertyChanged.ChangeAndNotify(this, ref selectedCountryCode, value);
			}
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= OnCultureChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

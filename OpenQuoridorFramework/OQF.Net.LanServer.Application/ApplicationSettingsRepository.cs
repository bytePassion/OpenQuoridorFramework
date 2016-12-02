using OQF.Net.LanServer.Contracts;

namespace OQF.Net.LanServer.Application
{
	internal class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private string selectedLanguageCode;

		public ApplicationSettingsRepository()
		{			
			selectedLanguageCode = Properties.Settings.Default.SelectedLanguageCode;
		}
		
		public string SelectedLanguageCode
		{
			get { return selectedLanguageCode; }
			set
			{
				selectedLanguageCode = value;
				Properties.Settings.Default.SelectedLanguageCode = value;
				Properties.Settings.Default.Save();
			}
		}
	}
}

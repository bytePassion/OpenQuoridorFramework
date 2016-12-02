using OQF.Net.DesktopClient.Contracts;

namespace OQF.Net.DesktopClient.Application
{
	internal class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private string lastConnectedServerAddress;
		private string lastUsedPlayerName;
		private string selectedLanguageCode;

		public ApplicationSettingsRepository()
		{
			lastConnectedServerAddress = Properties.Settings.Default.LastConnectedServerAddress;
			lastUsedPlayerName         = Properties.Settings.Default.LastUsedPlayerName;
			selectedLanguageCode       = Properties.Settings.Default.SelectedLanguageCode;
		}
		
		public string LastConnectedServerAddress
		{
			get { return lastConnectedServerAddress; }
			set
			{
				lastConnectedServerAddress = value;
				Properties.Settings.Default.LastConnectedServerAddress = value;
				Properties.Settings.Default.Save();
			}
		}

		public string LastUsedPlayerName
		{
			get { return lastUsedPlayerName; }
			set
			{
				lastUsedPlayerName = value;
				Properties.Settings.Default.LastUsedPlayerName = value;
				Properties.Settings.Default.Save();
			}
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

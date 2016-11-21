using OQF.ReplayViewer.Contracts;

namespace OQF.ReplayViewer.Application
{
	internal class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private string lastPlayedReplayString;
		private string selectedLanguageCode;

		public ApplicationSettingsRepository()
		{
			lastPlayedReplayString = Properties.Settings.Default.LastPlayedReplayString;
			selectedLanguageCode   = Properties.Settings.Default.SelectedLanguageCode;
		}

		public string LastPlayedReplayString
		{
			get { return lastPlayedReplayString; }
			set
			{
				lastPlayedReplayString = value;
				Properties.Settings.Default.LastPlayedReplayString = value;
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

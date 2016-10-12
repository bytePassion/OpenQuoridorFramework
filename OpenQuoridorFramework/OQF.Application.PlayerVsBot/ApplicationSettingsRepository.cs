using OQF.PlayerVsBot.Contracts.Settings;

namespace OQF.PlayerVsBot.Application
{
	public class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private bool isDebugSectionExpanded;
		private bool isProgressSecionExpanded;
		private string selectedLanguageCode;
		private string lastUsedBotPath;

		public ApplicationSettingsRepository()
		{
			lastUsedBotPath          = Properties.Settings.Default.LastUsedBotPath;
			selectedLanguageCode     = Properties.Settings.Default.SelectedLanguageCode;
			isProgressSecionExpanded = Properties.Settings.Default.IsProgressSecionExpanded;
			isDebugSectionExpanded   = Properties.Settings.Default.IsDebugSectionExpanded;
		}
				
		public string LastUsedBotPath
		{
			get { return lastUsedBotPath; }
			set
			{
				lastUsedBotPath = value;
				Properties.Settings.Default.LastUsedBotPath = value;
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

		public bool IsProgressSecionExpanded
		{
			get { return isProgressSecionExpanded; }
			set
			{
				isProgressSecionExpanded = value;
				Properties.Settings.Default.IsProgressSecionExpanded = value;
				Properties.Settings.Default.Save();
			}
		}

		public bool IsDebugSectionExpanded
		{
			get { return isDebugSectionExpanded; }
			set
			{
				isDebugSectionExpanded = value;
				Properties.Settings.Default.IsDebugSectionExpanded = value;
				Properties.Settings.Default.Save();
			}
		}
	}
}
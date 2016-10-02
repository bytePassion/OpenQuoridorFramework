namespace OQF.PlayerVsBot.Services.SettingsRepository
{
	public class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private bool isDebugSectionExpanded;
		private bool isProgressSecionExpanded;
		private string selectedLanguageCode;
		private string lastUsedBotPath;

		public ApplicationSettingsRepository()
		{
//			lastUsedBotPath          = Properties.Settings.Default.f;
//			selectedLanguageCode     = Properties.Settings.Default.SelectedLanguageCode;
//			isProgressSecionExpanded = Properties.Settings.Default.IsProgressSecionExpanded;
//			isDebugSectionExpanded   = Properties.Settings.Default.IsDebugSectionExpanded;
		}
				
		public string LastUsedBotPath
		{
			get
			{
				return string.Empty;
				//return lastUsedBotPath;
			}
			set
			{
//				lastUsedBotPath = value;
//				Properties.Settings.Default.LastUsedBotPath = value;
//				Properties.Settings.Default.Save();				
			}
		}		

		public string SelectedLanguageCode
		{
			get
			{
				return "de";
				//return selectedLanguageCode;
			}
			set
			{
//				selectedLanguageCode = value;
//				Properties.Settings.Default.SelectedLanguageCode = value;
//				Properties.Settings.Default.Save();
			}
		}

		public bool IsProgressSecionExpanded
		{
			get
			{
				return true;
				//return isProgressSecionExpanded;
			}
			set
			{
//				isProgressSecionExpanded = value;
//				Properties.Settings.Default.IsProgressSecionExpanded = value;
//				Properties.Settings.Default.Save();
			}
		}

		public bool IsDebugSectionExpanded
		{
			get
			{
				return false;
				//return isDebugSectionExpanded;
			}
			set
			{
//				isDebugSectionExpanded = value;
//				Properties.Settings.Default.IsDebugSectionExpanded = value;
//				Properties.Settings.Default.Save();
			}
		}
	}
}
namespace OQF.PlayerVsBot.Services.SettingsRepository
{
	internal interface IApplicationSettingsRepository
	{		
		string LastUsedBotPath      { get; set; }
		string SelectedLanguageCode { get; set; }
		
		bool IsProgressSecionExpanded { get; set; }
		bool IsDebugSectionExpanded   { get; set; }
	}
}
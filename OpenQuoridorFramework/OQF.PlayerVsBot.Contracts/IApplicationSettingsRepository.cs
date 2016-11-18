namespace OQF.PlayerVsBot.Contracts
{
	public interface IApplicationSettingsRepository
	{		
		string LastUsedBotPath      { get; set; }
		string SelectedLanguageCode { get; set; }
		
		bool IsProgressSecionExpanded { get; set; }
		bool IsDebugSectionExpanded   { get; set; }
	}
}
namespace OQF.Net.DesktopClient.Contracts
{
	public interface IApplicationSettingsRepository
	{
		string LastConnectedServerAddress { get; set; }
		string LastUsedPlayerName         { get; set; }
		string SelectedLanguageCode       { get; set; }
	}
}

namespace OQF.ReplayViewer.Contracts
{
	public interface IApplicationSettingsRepository
	{
		string LastPlayedReplayString { get; set; }
		string SelectedLanguageCode   { get; set; }
	}
}

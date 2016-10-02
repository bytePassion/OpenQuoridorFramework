namespace OQF.ReplayViewer.Services
{
	public interface ILastPlayedReplayService
	{
		string GetLastReplay();
		void SaveLastReplay(string botPath);
	}
}

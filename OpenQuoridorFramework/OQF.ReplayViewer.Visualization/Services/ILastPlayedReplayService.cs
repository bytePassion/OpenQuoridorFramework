namespace OQF.ReplayViewer.Visualization.Services
{
	public interface ILastPlayedReplayService
	{
		string GetLastReplay();
		void SaveLastReplay(string botPath);
	}
}

namespace OQF.Visualization.ReplayViewer.Services
{
	public interface ILastPlayedReplayService
	{
		string GetLastReplay();
		void SaveLastReplay(string botPath);
	}
}

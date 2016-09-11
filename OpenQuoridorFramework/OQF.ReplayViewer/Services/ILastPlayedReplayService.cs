namespace OQF.ReplayViewer.Services
{
	internal interface ILastPlayedReplayService
	{
		string GetLastReplay();
		void SaveLastReplay(string botPath);
	}
}

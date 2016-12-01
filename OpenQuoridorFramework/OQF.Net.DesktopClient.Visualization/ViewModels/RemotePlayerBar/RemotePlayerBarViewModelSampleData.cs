using System.ComponentModel;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar
{
	internal class RemotePlayerBarViewModelSampleData : IRemotePlayerBarViewModel
	{
		public RemotePlayerBarViewModelSampleData()
		{
			IsGameInitiator = true;
			WallsLeft = "7";
			WallsLeftLabelCaption = "Walls";			
		}

		public bool? IsGameInitiator { get; }
		public string WallsLeft { get; }
		public string WallsLeftLabelCaption { get; }		

		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
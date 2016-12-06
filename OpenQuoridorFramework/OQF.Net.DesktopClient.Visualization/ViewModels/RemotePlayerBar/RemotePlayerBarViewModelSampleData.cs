using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.RemotePlayerBar
{
	internal class RemotePlayerBarViewModelSampleData : IRemotePlayerBarViewModel
	{
		public RemotePlayerBarViewModelSampleData()
		{
			IsGameInitiator = true;
			WallsLeft = "7";
			WallsLeftLabelCaption = "Walls";
			PlayerName = "opponend player";
		}

		public bool? IsGameInitiator { get; }
		public string WallsLeft { get; }
		public string PlayerName { get; }
		public string WallsLeftLabelCaption { get; }		

		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
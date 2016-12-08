using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	internal class LocalPlayerBarViewModelSampleData : ILocalPlayerBarViewModel
	{

		public LocalPlayerBarViewModelSampleData()
		{
			IsGameInitiator = true;
			WallsLeft = "7";
			WallsLeftLabelCaption = "Walls";
			CapitulateButtonCaption = "Capitulate";
			PlayerName = "localPlayer";
			PlayerStatus = " - it's your turn";
		}

		public ICommand Capitulate => null;
		public bool? IsGameInitiator { get; }
		public string WallsLeft { get; }
		public string PlayerName { get; }
		public string PlayerStatus { get; }
		public string WallsLeftLabelCaption { get; }
		public string CapitulateButtonCaption { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
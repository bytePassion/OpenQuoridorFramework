using System.ComponentModel;
using System.Windows.Input;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.LocalPlayerBar
{
	internal class LocalPlayerBarViewModelSampleData : ILocalPlayerBarViewModel
	{

		public LocalPlayerBarViewModelSampleData()
		{
			IsGameInitiator = true;
			WallsLeft = 7;
		}

		public ICommand Capitulate => null;
		public bool IsGameInitiator { get; }
		public int WallsLeft { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
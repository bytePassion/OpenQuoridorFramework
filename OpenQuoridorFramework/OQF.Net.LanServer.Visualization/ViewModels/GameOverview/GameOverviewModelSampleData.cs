using System.Collections.ObjectModel;
using System.ComponentModel;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview.Helper;

#pragma warning disable 0067

namespace OQF.Net.LanServer.Visualization.ViewModels.GameOverview
{
	internal class GameOverviewModelSampleData : IGameOverviewModel
	{
		public GameOverviewModelSampleData()
		{
			CurrentGames = new ObservableCollection<GameDisplayData>
			{
				new GameDisplayData("game1", "p1", ""),
				new GameDisplayData("game2", "p2", ""),
				new GameDisplayData("game3", "p3", "p6"),
				new GameDisplayData("game4", "p4", "p7"),
				new GameDisplayData("game5", "p5", ""),
			};
		}

		public ObservableCollection<GameDisplayData> CurrentGames { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}
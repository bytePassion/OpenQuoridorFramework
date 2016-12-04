using System.Collections.ObjectModel;
using Lib.Wpf.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview.Helper;

namespace OQF.Net.LanServer.Visualization.ViewModels.GameOverview
{
	public interface IGameOverviewModel : IViewModel
	{
		ObservableCollection<GameDisplayData> CurrentGames { get; }
	}
}
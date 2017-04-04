using System.Collections.ObjectModel;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Net.LanServer.Visualization.ViewModels.GameOverview.Helper;

namespace OQF.Net.LanServer.Visualization.ViewModels.GameOverview
{
	public interface IGameOverviewModel : IViewModel
	{
		ObservableCollection<GameDisplayData> CurrentGames { get; }
	}
}
using System.Collections.ObjectModel;
using System.Windows;
using OQF.Contest.Contracts.GameElements;
using OQF.Tools.WpfTools.ViewModelBase;

namespace OQF.SingleGameVisualization.ViewModels.Board
{
	internal interface IBoardViewModel : IViewModel
	{
		ObservableCollection<Wall>        VisibleWalls   { get; }
		ObservableCollection<PlayerState> VisiblePlayers { get; }		

		Size BoardSize { get; set; }
	}
}
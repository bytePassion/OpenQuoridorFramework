using System.Collections.ObjectModel;
using System.Windows;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.ReplayViewer.ViewModels.Board
{
	internal interface IBoardViewModel : IViewModel
	{
		ObservableCollection<Wall>        VisibleWalls   { get; }
		ObservableCollection<PlayerState> VisiblePlayers { get; }		

		Size BoardSize { get; set; }
	}
}
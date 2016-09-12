using System.Collections.ObjectModel;
using System.Windows;
using OQF.Contest.Contracts.GameElements;
using WpfLib.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.Board
{
	internal interface IBoardViewModel : IViewModel
	{
		ObservableCollection<Wall>        VisibleWalls   { get; }
		ObservableCollection<PlayerState> VisiblePlayers { get; }		

		Size BoardSize { get; set; }
	}
}
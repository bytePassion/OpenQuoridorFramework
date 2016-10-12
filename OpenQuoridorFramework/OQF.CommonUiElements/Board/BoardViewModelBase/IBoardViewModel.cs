using System.Collections.ObjectModel;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.BoardViewModelBase
{
	public interface IBoardViewModel : IViewModel
	{
		ObservableCollection<Wall>        VisibleWalls   { get; }
		ObservableCollection<PlayerState> VisiblePlayers { get; }		

		Lib.SemanicTypes.Size BoardSize { get; set; }
	}
}
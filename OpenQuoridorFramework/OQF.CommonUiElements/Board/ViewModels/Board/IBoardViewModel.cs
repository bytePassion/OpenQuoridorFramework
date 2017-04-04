using System.Collections.ObjectModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.ViewModels.Board
{
	public interface IBoardViewModel : IViewModel
	{
		ObservableCollection<Wall>        VisibleWalls   { get; }
		ObservableCollection<PlayerState> VisiblePlayers { get; }		

		Size BoardSize { get; set; }
	}
}
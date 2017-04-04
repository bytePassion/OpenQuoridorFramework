using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ViewModelBase;
using OQF.Bot.Contracts.GameElements;

namespace OQF.CommonUiElements.Board.ViewModels.BoardPlacement
{
	public interface IBoardPlacementViewModel : IViewModel
	{
		ICommand BoardClick { get; }
		
		ObservableCollection<PlayerState> PossibleMoves       { get; }		
		ObservableCollection<Wall>        PotentialPlacedWall { get; }

		Point MousePosition { set; }
		Size BoardSize { set; get; }
	}
}
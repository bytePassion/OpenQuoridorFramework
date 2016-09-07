using System.Collections.ObjectModel;
using System.Windows.Input;
using OQF.Contest.Contracts.GameElements;
using OQF.Tools.SemanticTypes;
using OQF.Tools.WpfTools.ViewModelBase;
using Size = System.Windows.Size;

namespace OQF.SingleGameVisualization.ViewModels.BoardPlacement
{
	internal interface IBoardPlacementViewModel : IViewModel
	{
		ICommand BoardClick { get; }
		
		ObservableCollection<PlayerState> PossibleMoves       { get; }		
		ObservableCollection<Wall>        PotentialPlacedWall { get; }

		Point MousePosition { set; }
		Size BoardSize { set; get; }
	}
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using QCF.Contest.Contracts.GameElements;
using QCF.Tools.SemanticTypes;
using QCF.Tools.WpfTools.ViewModelBase;
using Size = System.Windows.Size;

namespace QCF.SingleGameVisualization.ViewModels.BoardPlacement
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
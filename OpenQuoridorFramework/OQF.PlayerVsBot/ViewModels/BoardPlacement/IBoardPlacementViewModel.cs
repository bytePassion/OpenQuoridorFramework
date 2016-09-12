using System.Collections.ObjectModel;
using System.Windows.Input;
using OQF.Contest.Contracts.GameElements;
using SemanicTypesLib;
using WpfLib.ViewModelBase;
using Size = System.Windows.Size;

namespace OQF.PlayerVsBot.ViewModels.BoardPlacement
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
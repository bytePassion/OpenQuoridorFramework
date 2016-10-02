using System.Collections.ObjectModel;
using System.Windows.Input;
using Lib.SemanicTypes;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using Size = System.Windows.Size;

namespace OQF.PlayerVsBot.ViewModels.BoardPlacement
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Lib.FrameworkExtension;
using Lib.SemanicTypes;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using Size = Lib.SemanicTypes.Size;

namespace OQF.CommonUiElements.Board.BoardViewModel
{
	public class BoardViewModel : ViewModel, IBoardViewModel
	{
		private readonly IBoardStateProvider boardStateProvider;
				
		private Size boardSize;

		public BoardViewModel(IBoardStateProvider boardStateProvider)
		{
			this.boardStateProvider = boardStateProvider;


			VisiblePlayers = new ObservableCollection<PlayerState>();
			VisibleWalls   = new ObservableCollection<Wall>();

			boardStateProvider.NewBoardStateAvailable += OnNewBoardStateAvailable;
			OnNewBoardStateAvailable(boardStateProvider.CurrentBoardState);
									
			BoardSize = new Size(new Width(100), new Height(100));
		}
		
		private void OnNewBoardStateAvailable(BoardState newBoardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				VisiblePlayers.Clear();
				VisibleWalls.Clear();

				if (newBoardState != null)
				{
					newBoardState.PlacedWalls.Do(VisibleWalls.Add);

					VisiblePlayers.Add(newBoardState.TopPlayer);
					VisiblePlayers.Add(newBoardState.BottomPlayer);
				}
			});			
		}		

		public ObservableCollection<Wall>        VisibleWalls   { get; }
		public ObservableCollection<PlayerState> VisiblePlayers { get; }

		public Size BoardSize
		{
			get { return boardSize; }
			set { PropertyChanged.ChangeAndNotify(this, ref boardSize, value); }
		}

		protected override void CleanUp()
		{
			boardStateProvider.NewBoardStateAvailable -= OnNewBoardStateAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

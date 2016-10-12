using System.Collections.ObjectModel;
using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.SemanicTypes;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.GameElements;
using OQF.CommonUiElements.Board.BoardViewModelBase;
using OQF.PlayerVsBot.Visualization.Services;

namespace OQF.PlayerVsBot.Visualization.ViewModels.Board
{
	public class BoardViewModel : ViewModel, IBoardViewModel
	{
		private readonly IGameService gameService;
				
		private Size boardSize;

		public BoardViewModel(IGameService gameService)
		{
			this.gameService = gameService;

			VisiblePlayers = new ObservableCollection<PlayerState>();
			VisibleWalls   = new ObservableCollection<Wall>();

			gameService.NewBoardStateAvailable += OnDisplayedBoardStateVariableChanged;
			OnDisplayedBoardStateVariableChanged(gameService.CurrentBoardState);
									
			BoardSize = new Size(new Width(100), new Height(100));			
		}
		
		private void OnDisplayedBoardStateVariableChanged(BoardState newBoardState)
		{
			VisiblePlayers.Clear();
			VisibleWalls.Clear();

			if (newBoardState != null)
			{
				newBoardState.PlacedWalls.Do(VisibleWalls.Add);

				VisiblePlayers.Add(newBoardState.TopPlayer);
				VisiblePlayers.Add(newBoardState.BottomPlayer);
			}
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
			gameService.NewBoardStateAvailable -= OnDisplayedBoardStateVariableChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

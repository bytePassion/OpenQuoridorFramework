using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using QCF.Contest.Contracts.GameElements;
using QCF.SingleGameVisualization.Services;
using QCF.Tools.FrameworkExtensions;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.Board
{
	internal class BoardViewModel : ViewModel, IBoardViewModel
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
									
			BoardSize = new Size(100,100);
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

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

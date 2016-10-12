using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.SemanicTypes;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.GameEngine.Contracts.Enums;
using OQF.GameEngine.Contracts.Factories;
using OQF.PlayerVsBot.Visualization.Services;

namespace OQF.PlayerVsBot.Visualization.ViewModels.BoardPlacement
{
	public class BoardPlacementViewModel : ViewModel, IBoardPlacementViewModel
	{
		private readonly IGameService gameService;
		private readonly IGameFactory gameFactory;
		private Point currentMousePosition;
		private Size boardSize;		

		private IEnumerable<Wall> allPossibleWalls;

		public BoardPlacementViewModel(IGameService gameService, IGameFactory gameFactory)
		{
			this.gameService = gameService;
			this.gameFactory = gameFactory;			

			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			gameService.WinnerAvailable += OnWinnerAvailable;

			BoardClick = new Command(HandleBoardClick);

			PossibleMoves = new ObservableCollection<PlayerState>();
			PotentialPlacedWall = new ObservableCollection<Wall>();
		}

		private void OnWinnerAvailable(Player player, WinningReason winningReason, Move illegalMove)
		{
			DisablePlacement();
		}

		private void DisablePlacement()
		{
			PossibleMoves.Clear();
			PotentialPlacedWall.Clear();
			allPossibleWalls = null;
		}		

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			if (boardState?.CurrentMover.PlayerType == PlayerType.BottomPlayer)
			{
				var boardAnalysis = gameFactory.GetGameAnalysis(boardState);

				allPossibleWalls = boardAnalysis.GetPossibleWalls();				

				boardAnalysis.GetPossibleMoves()
							 .Select(move => new PlayerState(null, move, -1))
							 .Do(PossibleMoves.Add);
			}
			else
			{
				DisablePlacement();
			}						
		}

		public ICommand BoardClick { get; }
		public ObservableCollection<PlayerState> PossibleMoves       { get; }
		public ObservableCollection<Wall>        PotentialPlacedWall { get; }


		public Point MousePosition
		{
			set
			{
				currentMousePosition = value;

				if (gameService.CurrentBoardState?.CurrentMover.PlayerType == PlayerType.BottomPlayer)
				{
					if (gameService.CurrentBoardState.BottomPlayer.WallsToPlace > 0)
						CheckIfMouseIsOverPossibleWall();
				}				
			}
		}
		
		public Size BoardSize
		{
			get { return boardSize; }
			set { PropertyChanged.ChangeAndNotify(this, ref boardSize, value); }
		}

		private void HandleBoardClick()
		{
			if (PotentialPlacedWall.Count > 0)
			{
				gameService.ReportHumanMove(new WallMove(PotentialPlacedWall[0]));
				return;
			}

			var fieldToMove = IsMouseOverPotentialMoveField();

			if (fieldToMove.HasValue)
			{
				gameService.ReportHumanMove(new FigureMove(fieldToMove.Value));				
			}
		}

		private void CheckIfMouseIsOverPossibleWall()
		{
			if (currentMousePosition.XCoord < 0 || currentMousePosition.XCoord > boardSize.Width ||
				currentMousePosition.YCoord < 0 || currentMousePosition.YCoord > boardSize.Height)
			{
				PotentialPlacedWall.Clear();
				return;
			}

			var cellWidth  = boardSize.Width/11.4;
			var cellHeight = boardSize.Height/11.4;

			if (allPossibleWalls != null)
			{
				foreach (var possibleWall in allPossibleWalls)
				{
					var xMin = possibleWall.Orientation == WallOrientation.Horizontal
						? (double) possibleWall.TopLeft.XCoord*1.3*cellWidth
						: ((double) possibleWall.TopLeft.XCoord + 1)*1.3*cellWidth - (0.3*cellWidth);

					var xMax = xMin + (possibleWall.Orientation == WallOrientation.Horizontal
						           ? 2.3*cellWidth
						           : 0.3*cellWidth);

					var yMin = possibleWall.Orientation == WallOrientation.Horizontal
						? ((double) possibleWall.TopLeft.YCoord + 1)*1.3*cellHeight - (0.3*cellHeight)
						: (double) possibleWall.TopLeft.YCoord*1.3*cellHeight;

					var yMax = yMin + (possibleWall.Orientation == WallOrientation.Horizontal
						           ? 0.3*cellHeight
						           : 2.3*cellHeight);

					if (currentMousePosition.XCoord >= xMin && currentMousePosition.XCoord <= xMax &&
					    currentMousePosition.YCoord >= yMin && currentMousePosition.YCoord <= yMax)
					{
						if (PotentialPlacedWall.Count > 0 && PotentialPlacedWall[0] != possibleWall)
						{
							PotentialPlacedWall.Clear();
						}

						if (PotentialPlacedWall.Count == 0)
							PotentialPlacedWall.Add(possibleWall);

						return;
					}
				}
			}

			PotentialPlacedWall.Clear();
		}

		private FieldCoordinate? IsMouseOverPotentialMoveField()
		{
			var cellWidth  = boardSize.Width/11.4;
			var cellHeight = boardSize.Height/11.4;

			foreach (var moveField in PossibleMoves)
			{
				var xMin = (double)moveField.Position.XCoord * 1.3 * cellWidth;
				var xMax = xMin + cellWidth;
				var yMin = (double)moveField.Position.YCoord * 1.3 * cellHeight;
				var yMax = yMin + cellHeight;

				if (currentMousePosition.XCoord >= xMin && currentMousePosition.XCoord <= xMax &&
					currentMousePosition.YCoord >= yMin && currentMousePosition.YCoord <= yMax)
				{
					return moveField.Position;
				}
			}

			return null;
		}

		protected override void CleanUp()
		{
			gameService.NewBoardStateAvailable -= OnNewBoardStateAvailable;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

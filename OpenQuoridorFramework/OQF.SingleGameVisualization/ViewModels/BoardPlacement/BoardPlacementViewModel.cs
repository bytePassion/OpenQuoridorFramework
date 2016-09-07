using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OQF.Contest.Contracts.Coordination;
using OQF.Contest.Contracts.GameElements;
using OQF.Contest.Contracts.Moves;
using OQF.GameEngine.Contracts;
using OQF.SingleGameVisualization.Services;
using OQF.Tools.FrameworkExtensions;
using OQF.Tools.SemanticTypes;
using OQF.Tools.WpfTools.Commands;
using OQF.Tools.WpfTools.ViewModelBase;
using Size = System.Windows.Size;

namespace OQF.SingleGameVisualization.ViewModels.BoardPlacement
{
	internal class BoardPlacementViewModel : ViewModel, IBoardPlacementViewModel
	{
		private readonly IGameService gameService;
		private readonly IGameFactory gameFactory;
		private Point currentMousePosition;
		private Size boardSize;		

		private IList<Wall> allPossibleWalls;

		public BoardPlacementViewModel(IGameService gameService, IGameFactory gameFactory)
		{
			this.gameService = gameService;
			this.gameFactory = gameFactory;

			allPossibleWalls = GenerateAllPossibleWalls();

			gameService.NewBoardStateAvailable += OnNewBoardStateAvailable;

			BoardClick = new Command(HandleBoardClick);

			PossibleMoves = new ObservableCollection<PlayerState>();
			PotentialPlacedWall = new ObservableCollection<Wall>();
		}

		private IList<Wall> GenerateAllPossibleWalls()
		{
			var resultList = new List<Wall>(128);

			for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
			{
				for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
				{
					resultList.Add(new Wall(new FieldCoordinate(xCoord, yCoord), WallOrientation.Horizontal));
					resultList.Add(new Wall(new FieldCoordinate(xCoord, yCoord), WallOrientation.Vertical));
				}
			}

			return resultList;
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			if (boardState?.CurrentMover.PlayerType == PlayerType.BottomPlayer)
			{
				var boardAnalysis = gameFactory.GetGameAnalysis(boardState);

//				allPossibleWalls = new List<Wall>(boardAnalysis.GetPossibleWalls());
				
				boardAnalysis.GetPossibleMoves()
							 .Select(move => new PlayerState(null, move, -1))
							 .Do(PossibleMoves.Add);
			}
			else
			{
				PossibleMoves.Clear();
				PotentialPlacedWall.Clear();
			//	allPossibleWalls = null;
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
				gameService.ReportHumanMove(new WallMove(gameService.CurrentBoardState,
														 gameService.CurrentBoardState.CurrentMover,
														 PotentialPlacedWall[0]));
				return;
			}

			var fieldToMove = IsMouseOverPotentialMoveField();

			if (fieldToMove.HasValue)
			{
				gameService.ReportHumanMove(new FigureMove(gameService.CurrentBoardState,
														   gameService.CurrentBoardState.CurrentMover,
														   fieldToMove.Value));				
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

			foreach (var possibleWall in allPossibleWalls)
			{				
				var xMin = possibleWall.Orientation == WallOrientation.Horizontal
								?  (double)possibleWall.TopLeft.XCoord      * 1.3 * cellWidth
								: ((double)possibleWall.TopLeft.XCoord + 1) * 1.3 * cellWidth - (0.3 * cellWidth);

				var xMax = xMin + (possibleWall.Orientation == WallOrientation.Horizontal
										? 2.3*cellWidth
										: 0.3*cellWidth);

				var yMin = possibleWall.Orientation == WallOrientation.Horizontal
								? ((double)possibleWall.TopLeft.YCoord + 1) * 1.3 * cellHeight - (0.3*cellHeight)
								:  (double)possibleWall.TopLeft.YCoord      * 1.3 * cellHeight;

				var yMax = yMin + (possibleWall.Orientation == WallOrientation.Horizontal
										? 0.3 * cellHeight
										: 2.3 * cellHeight);

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

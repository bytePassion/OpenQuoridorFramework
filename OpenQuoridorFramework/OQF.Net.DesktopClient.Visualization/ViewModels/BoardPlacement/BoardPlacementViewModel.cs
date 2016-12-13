using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.AnalysisAndProgress.Analysis;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.ViewModels.BoardPlacement;
using OQF.Net.DesktopClient.Contracts;
using OQF.Utils.Enum;
using Point = Lib.SemanicTypes.Point;
using Size = Lib.SemanicTypes.Size;

namespace OQF.Net.DesktopClient.Visualization.ViewModels.BoardPlacement
{
	public class BoardPlacementViewModel : ViewModel, IBoardPlacementViewModel
	{		
		private readonly INetworkGameService networkGameService;

		private Point currentMousePosition;
		private Size boardSize;		

		private IEnumerable<Wall> allPossibleWalls;

		public BoardPlacementViewModel(INetworkGameService networkGameService)
		{
			this.networkGameService = networkGameService;
		
			networkGameService.NewBoardStateAvailable += OnNewBoardStateAvailable;
			networkGameService.GameOver               += OnGameOver;

			BoardClick = new Command(HandleBoardClick);

			PossibleMoves = new ObservableCollection<PlayerState>();
			PotentialPlacedWall = new ObservableCollection<Wall>();

			IsPlacementEnabled = false;
			IsWallPlacementPossible = false;
		}

		private void OnNewBoardStateAvailable(BoardState boardState)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (boardState != null && networkGameService.ClientPlayer != null)
				{
					if (boardState.CurrentMover.PlayerType == networkGameService.ClientPlayer.PlayerType)
					{
						var boardAnalysis = PlayerAnalysis.GetResult(boardState, networkGameService.ClientPlayer.PlayerType);

						allPossibleWalls = boardAnalysis.PossibleWalls;

						boardAnalysis.PossibleMoves
							.Select(move => new PlayerState(null, move, -1))
							.Do(PossibleMoves.Add);

						IsPlacementEnabled = true;

						IsWallPlacementPossible = networkGameService.ClientPlayer.PlayerType == PlayerType.BottomPlayer
							? boardState.BottomPlayer.WallsToPlace > 0
							: boardState.TopPlayer.WallsToPlace > 0;
					}
					else
					{
						DisablePlacement();
					}
				}
				else
				{
					DisablePlacement();
				}

			});
		}

		private bool IsPlacementEnabled { get; set; }
		private bool IsWallPlacementPossible { get; set; }

		private void OnGameOver(bool b, WinningReason winningReason)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				DisablePlacement();
			});
		}
		
		private void DisablePlacement()
		{
			PossibleMoves.Clear();
			PotentialPlacedWall.Clear();
			allPossibleWalls = null;
			IsPlacementEnabled = false;
			IsWallPlacementPossible = false;
		}		

		

		public ICommand BoardClick { get; }
		public ObservableCollection<PlayerState> PossibleMoves       { get; }
		public ObservableCollection<Wall>        PotentialPlacedWall { get; }


		public Point MousePosition
		{
			set
			{
				currentMousePosition = value;

				if (IsPlacementEnabled && IsWallPlacementPossible)
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
				networkGameService.SubmitMove(new WallMove(PotentialPlacedWall[0]));
				DisablePlacement();
				return;
			}

			var fieldToMove = IsMouseOverPotentialMoveField();

			if (fieldToMove.HasValue)
			{
				networkGameService.SubmitMove(new FigureMove(fieldToMove.Value));
				DisablePlacement();
			}
		}

		private void CheckIfMouseIsOverPossibleWall()
		{
			if (currentMousePosition != null)
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
			networkGameService.NewBoardStateAvailable -= OnNewBoardStateAvailable;
			networkGameService.GameOver               -= OnGameOver;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

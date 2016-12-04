using System;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.CommonUiElements.Board.ViewModels;
using OQF.Utils.Enum;

namespace OQF.PlayerVsBot.Contracts
{
	public interface IGameService : IBoardStateProvider
	{		
		event Action<string>                      NewDebugMsgAvailable;
		event Action<Player, WinningReason, Move> WinnerAvailable;
		event Action<GameStatus>                  NewGameStatusAvailable;

		GameStatus CurrentGameStatus { get; }

		void CreateGame(IQuoridorBot uninitializedBot, 
						string botName, 
						GameConstraints gameConstraints, 
						QProgress initialProgress = null);

		void ReportHumanMove(Move move);
		void StopGame();
	}
}
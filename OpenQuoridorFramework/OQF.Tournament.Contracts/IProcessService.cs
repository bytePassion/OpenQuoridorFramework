using System;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.Moves;

namespace OQF.Tournament.Contracts
{
	public interface IProcessService : IDisposable
    {
        Guid CreateBotProcess();

        void LoadBot(Guid internalProcessId, string dllPath, PlayerType playerType, Action loadingFinishedCallback);
        void InitBot(Guid internalProcessId, GameConstraints gameConstraints, PlayerType playerType, Action initFinishedCallback);
        void DoMove(Guid internalProcessId, QProgress progress, Action<Move> nextBotMove);

        void KillBotProcess(Guid internalProcessId);

    }
}
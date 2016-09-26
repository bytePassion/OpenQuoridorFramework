using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Enums;

namespace OQF.GameEngine.Analysis
{
	internal class ProgressFileVerifier : IProgressFileVerifier
	{
		public FileVerificationResult Verify(string progressText)
		{
			var topPlayer    = new Player(PlayerType.TopPlayer);
			var bottomPlayer = new Player(PlayerType.BottomPlayer);

			

			return FileVerificationResult.ValidFile;
		}
	}
}

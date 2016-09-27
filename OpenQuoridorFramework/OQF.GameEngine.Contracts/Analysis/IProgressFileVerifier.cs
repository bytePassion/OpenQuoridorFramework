using OQF.GameEngine.Contracts.Enums;

namespace OQF.GameEngine.Contracts.Analysis
{
	public interface IProgressFileVerifier
	{
		FileVerificationResult Verify(string progressText, int maxMoves);
	}
}
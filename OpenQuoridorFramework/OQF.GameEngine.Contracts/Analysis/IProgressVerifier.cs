using OQF.GameEngine.Contracts.Enums;

namespace OQF.GameEngine.Contracts.Analysis
{
	public interface IProgressVerifier
	{
		ProgressVerificationResult Verify(string progressText, ProgressTextType textType, int maxMoves);
	}
}
using OQF.GameEngine.Contracts.Analysis;

namespace OQF.GameEngine.Contracts.Factories
{
	public interface IProgressVerifierFactory
	{
		IProgressVerifier CreateVerifier();
	}
}
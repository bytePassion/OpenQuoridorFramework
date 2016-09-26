using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Factories;
using OQF.GameEngine.Verifier;

namespace OQF.GameEngine.Factories
{
	public class ProgressFileVerifierFactory : IProgressFileVerifierFactory
	{
		public IProgressFileVerifier CreateVerifier()
		{
			return new ProgressFileVerifier();
		}
	}
}

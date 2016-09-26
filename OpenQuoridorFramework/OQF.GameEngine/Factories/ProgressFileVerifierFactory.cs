using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Factories;

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

using OQF.GameEngine.Analysis;
using OQF.GameEngine.Contracts.Analysis;
using OQF.GameEngine.Contracts.Factories;

namespace OQF.GameEngine.Factories
{
	public class ProgressVerifierFactory : IProgressVerifierFactory
	{
		public IProgressVerifier CreateVerifier()
		{
			return new ProgressVerifier();
		}
	}
}

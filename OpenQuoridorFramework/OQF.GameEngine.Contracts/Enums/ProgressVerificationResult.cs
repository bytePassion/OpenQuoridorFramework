namespace OQF.GameEngine.Contracts.Enums
{
	public enum ProgressVerificationResult
	{
		Valid,
		EmptyOrInvalid,
		ProgressContainsInvalidMove,
		ProgressContainsTerminatedGame,
		ProgressContainsMoreMovesThanAllowed
	}
}

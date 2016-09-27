namespace OQF.GameEngine.Contracts.Enums
{
	public enum FileVerificationResult
	{
		ValidFile,
		EmptyOrInvalidFile,
		FileContainsInvalidMove,
		FileContainsTerminatedGame,
		FileContainsMoreMovesThanAllowed
	}
}

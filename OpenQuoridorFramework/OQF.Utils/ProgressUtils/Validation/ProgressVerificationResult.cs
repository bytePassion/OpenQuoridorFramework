namespace OQF.Utils.ProgressUtils.Validation
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

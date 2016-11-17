namespace OQF.AnalysisAndProgress.ProgressUtils.Validation
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

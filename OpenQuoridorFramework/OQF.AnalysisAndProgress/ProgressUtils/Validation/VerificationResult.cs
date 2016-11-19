namespace OQF.AnalysisAndProgress.ProgressUtils.Validation
{
	public enum VerificationResult
	{
		Valid,
		EmptyOrInvalid,
		ProgressContainsInvalidMove,
		ProgressContainsTerminatedGame,
		ProgressContainsMoreMovesThanAllowed
	}
}
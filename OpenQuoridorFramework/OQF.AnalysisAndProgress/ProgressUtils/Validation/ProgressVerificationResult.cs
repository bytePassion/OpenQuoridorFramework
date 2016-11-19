namespace OQF.AnalysisAndProgress.ProgressUtils.Validation
{
	public class ProgressVerificationResult
	{
		public ProgressVerificationResult(VerificationResult result, string errorMessage = null)
		{
			Result = result;
			ErrorMessage = errorMessage;
		}

		public VerificationResult Result { get; }
		public string ErrorMessage { get; }
	}
}

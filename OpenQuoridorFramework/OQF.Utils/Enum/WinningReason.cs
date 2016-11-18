namespace OQF.Utils.Enum
{
	// TODO: auf lange sicht evtl in Bot.Contracts umziehen 
	// TODO: und dem bot auch mitteilen warum er gewonnen/verloren hat

	public enum WinningReason
	{
		RegularQuoridorWin,
		Capitulation,
		InvalidMove,
		ExceedanceOfThoughtTime,
		ExceedanceOfMaxMoves
	}
}

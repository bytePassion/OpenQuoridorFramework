﻿using Lib.FrameworkExtension;
using OQF.Bot.Contracts.Moves;

namespace OQF.Utils.ProgressUtils
{
	public static class UpdateQProgress
	{
		public static QProgress WithNewMove(QProgress progress, Move newMove)
		{
			return CreateQProgress.FromMoveList(progress.Moves.Append(newMove));
		}
	}
}
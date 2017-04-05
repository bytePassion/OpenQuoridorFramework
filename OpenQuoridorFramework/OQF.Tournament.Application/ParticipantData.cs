using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bytePassion.Lib.GeometryLib.Utils;
using OQF.Tournament.Contracts.DTO;

namespace OQF.Tournament.Application
{
	public class ParticipantData
	{
		private readonly TournamentParticipant participant;

		private readonly IList<TimeSpan> allTimeSpans;
		private readonly IList<int> winingMoveCounts;
		private readonly IList<int> loosingMoveCounts;
		private readonly IList<int> winningWallCounts;
		private readonly IList<int> loosingWallCounts;
		private int wins;
		private int losses;

		public ParticipantData(TournamentParticipant participant)
		{
			this.participant = participant;
			allTimeSpans = new List<TimeSpan>();
			winingMoveCounts = new List<int>();
			loosingMoveCounts = new List<int>();
			winningWallCounts = new List<int>();
			loosingWallCounts = new List<int>();
			wins = 0;
			losses = 0;
		}

		public void ReportMoveTime (TimeSpan moveTime)
		{
			allTimeSpans.Add(moveTime);
		}

		public void ReportMoveCount (int moveCount, bool win)
		{
			if (win)
				winingMoveCounts.Add(moveCount);
			else
				loosingMoveCounts.Add(moveCount);
		}

		public void ReportWallCount (int wallCount, bool win)
		{
			if (win)
				winningWallCounts.Add(wallCount);
			else
				loosingWallCounts.Add(wallCount);
		}

		public void ReportResult (bool win)
		{
			if (win)
				wins++;
			else
				losses++;
		}

		public string GetReport()
		{
			var sb = new StringBuilder();

			sb.AppendLine( "###############################################################################################################################");
			sb.AppendLine($"########  {participant.Name}   ####  {participant.DllPath}");
			sb.AppendLine( "###############################################################################################################################");
			sb.AppendLine();

			sb.AppendLine($"wins: {wins}");
			sb.AppendLine($"losses: {losses}");

			var summedTimeSpan = allTimeSpans.Select(ts => ts.TotalSeconds).Sum();
			sb.AppendLine($"summed moving time: {GeometryLibUtils.DoubleFormat(summedTimeSpan)}");

			var avarageTimeSpan = summedTimeSpan/allTimeSpans.Count;
			sb.AppendLine($"avarage moving time: {GeometryLibUtils.DoubleFormat(avarageTimeSpan)}");

			var summedPlacedWalls = winningWallCounts.Concat(loosingWallCounts).Sum();
			sb.AppendLine($"summed placed walls: {summedPlacedWalls}");

			if (winningWallCounts.Count + loosingWallCounts.Count > 0)
			{
				var avaragePlacedWalls = summedPlacedWalls/(double)(winningWallCounts.Count + loosingWallCounts.Count);
				sb.AppendLine($"avarage placed walls per game: {GeometryLibUtils.DoubleFormat(avaragePlacedWalls)}");
			}
			else
			{
				sb.AppendLine("avarage placed walls per game: 0");
			}		

			if (winningWallCounts.Any())
			{
				var avaragePlacedWallsWhenWinning = winningWallCounts.Sum()/(double)winningWallCounts.Count;
				sb.AppendLine($"avarage placed walls per game when winning: {avaragePlacedWallsWhenWinning}");
			}

			if (loosingWallCounts.Any())
			{
				var avaragePlacedWallsWhenLoosing = loosingWallCounts.Sum()/(double)loosingWallCounts.Count;
				sb.AppendLine($"avarage placed walls per game when loosing: {avaragePlacedWallsWhenLoosing}");
			}			

			var summedMoves = winingMoveCounts.Concat(loosingMoveCounts).Sum();
			sb.AppendLine($"summed moves: {summedMoves}");

			var avarageMoves = summedMoves/(double)(winingMoveCounts.Count + loosingMoveCounts.Count);
			sb.AppendLine($"avarage moves per game: {GeometryLibUtils.DoubleFormat(avarageMoves)}");

			if (winingMoveCounts.Any())
			{
				var avarageMoveCountWhileWinning = winingMoveCounts.Sum()/(double)winingMoveCounts.Count;
				sb.AppendLine($"avarage moves per game when winning: {avarageMoveCountWhileWinning}");
			}

			if (loosingMoveCounts.Any())
			{
				var avarageMoveCOuntWhileLoosing = loosingMoveCounts.Sum()/(double)loosingMoveCounts.Count;
				sb.AppendLine($"avarage moves per game when loosing: {avarageMoveCOuntWhileLoosing}");
			}

			sb.AppendLine();
			sb.AppendLine();

			return sb.ToString();
		}

	}
}
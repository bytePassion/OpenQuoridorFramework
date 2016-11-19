using System;
using System.Collections.Generic;
using System.Linq;
using OQF.AnalysisAndProgress.ProgressUtils;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace CodingDebug
{
	class Program
	{
		static void Main (string[] args)
		{
			for (int i = 0; i < 50; i++)
			{
				var moveList = new List<Move>();

				for (var x = XField.A; x <= XField.I; x++)
				{
					for (var y = YField.One; y >= YField.Nine; y--)
					{
						var coord = new FieldCoordinate(x, y);
						moveList.Add(new FigureMove(coord));
					}
				}

				var allWalls = new List<Wall>();				

				for (var xCoord = XField.A; xCoord < XField.I; xCoord++)
				{
					for (var yCoord = YField.Nine; yCoord < YField.One; yCoord++)
					{
						var coord = new FieldCoordinate(xCoord, yCoord);

						moveList.Add(new WallMove(new Wall(coord, WallOrientation.Horizontal)));						
						moveList.Add(new WallMove(new Wall(coord, WallOrientation.Vertical)));						
					}
				}

				moveList.Shuffle();

				var progress = CreateQProgress.FromMoveList(moveList);

				var compressedString = progress.Compressed;				

				var progressFromString = CreateQProgress.FromCompressedProgressString(compressedString);

				var newMoveList = progressFromString.Moves.ToList();

				var listsAreEqual = AreMoveListsEqual(moveList, newMoveList);

				if (listsAreEqual)
					Console.WriteLine($"ok [{compressedString.Substring(0, 10)}][{moveList.Count}]");
				else
					Console.WriteLine("error");
			}

			

			Console.ReadLine();
		}


		private static bool AreMoveListsEqual(IList<Move> l1, IList<Move> l2)
		{
			if (l1.Count != l2.Count)
				return false;

			for (int i = 0; i < l1.Count; i++)
			{
				var m1 = l1[i];
				var m2 = l2[i];

				if (!AreMovesEqual(m1, m2))
					return false;
			}

			return true;
		}

		private static bool AreMovesEqual(Move m1, Move m2)
		{
			if (m1 is FigureMove)
			{
				if (!(m2 is FigureMove))
					return false;

				return ((FigureMove) m1).NewPosition == ((FigureMove) m2).NewPosition;
			}

			if (m1 is WallMove)
			{
				if (!(m2 is WallMove))
					return false;

				return (((WallMove) m1).PlacedWall.TopLeft == ((WallMove) m2).PlacedWall.TopLeft) &&
				       (((WallMove) m1).PlacedWall.Orientation == ((WallMove) m2).PlacedWall.Orientation);
			}

			return false;
		}

	}

	public static class ListExtension
	{
		private static readonly Random Rng = new Random();

		public static void Shuffle<T> (this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = Rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}

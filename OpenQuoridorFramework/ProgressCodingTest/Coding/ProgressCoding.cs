using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace ProgressCodingTest.Coding
{
	internal static class ProgressCoding
	{

		public static BigInteger ConvertMoveListToBigInteger(IEnumerable<Move> moves)
		{
			var numberBuilder = new StringBuilder();

			foreach (var move in moves)
			{
				if (move is FigureMove)
				{
					var figureMove = (FigureMove) move;
					numberBuilder.Append((int) figureMove.NewPosition.XCoord + 1);
					numberBuilder.Append((int) figureMove.NewPosition.YCoord + 1);
				}
				else if (move is WallMove)
				{
					var wallMove = (WallMove) move;

					numberBuilder.Append("0");
					numberBuilder.Append((int) wallMove.PlacedWall.TopLeft.XCoord + 1);
					numberBuilder.Append((int) wallMove.PlacedWall.TopLeft.YCoord + 1);
					numberBuilder.Append(wallMove.PlacedWall.Orientation == WallOrientation.Horizontal ? "1" : "2");
				}
				else if (move is Capitulation)
				{
					numberBuilder.Append("0");
					numberBuilder.Append("0");
				}
			}

			return BigInteger.Parse(numberBuilder.ToString());
		}

		public static IEnumerable<Move> ConvertBigIntegerToMoveList(BigInteger codedMoves)
		{
			var codes = codedMoves.ToString();

			var resultList = new List<Move>();

			var index = 0;

			while (index < codes.Length)
			{
				if (codes[index] == '0' && codes[index+1] == '0')
				{
					resultList.Add(new Capitulation());
					break;
				}

				if (codes[index] == '0')
				{
					var x = GetX(codes[index + 1]);
					var y = GetY(codes[index + 2]);
					var o = GetO(codes[index + 3]);

					resultList.Add(new WallMove(new Wall(new FieldCoordinate(x, y), o)));

					index += 4;					
				}
				else
				{
					var x = GetX(codes[index]);
					var y = GetY(codes[index + 1]);

					resultList.Add(new FigureMove(new FieldCoordinate(x, y)));

					index += 2;
				}				
			}

			return resultList;
		}

		private static XField GetX(char c)
		{
			switch (c)
			{
				case '1': return XField.A;
				case '2': return XField.B;
				case '3': return XField.C;
				case '4': return XField.D;
				case '5': return XField.E;
				case '6': return XField.F;
				case '7': return XField.G;
				case '8': return XField.H;
				case '9': return XField.I;
			}

			throw new Exception();
		}

		private static YField GetY (char c)
		{
			switch (c)
			{
				case '1': return YField.Nine;
				case '2': return YField.Eight;
				case '3': return YField.Seven;
				case '4': return YField.Six;
				case '5': return YField.Five;
				case '6': return YField.Four;
				case '7': return YField.Three;
				case '8': return YField.Two;
				case '9': return YField.One;
			}

			throw new Exception();
		}

		private static WallOrientation GetO(char c)
		{
			switch (c)
			{
				case '1': return WallOrientation.Horizontal;
				case '2': return WallOrientation.Vertical;
			}

			throw new Exception();
		}
	}
}

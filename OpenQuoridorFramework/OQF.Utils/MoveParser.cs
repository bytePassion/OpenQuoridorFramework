using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;

namespace OQF.Utils
{
	public static class MoveParser
	{
		public static Move GetMove (string s)
		{
			var moveString = s.Trim();

			switch (moveString.Length)
			{
				case 2:
				{
					var xCoord = GetXCoord(moveString[0]);
					var yCoord = GetYCoord(moveString[1]);

					if (xCoord.HasValue && yCoord.HasValue)
					{
						var figurePosition = new FieldCoordinate(xCoord.Value, yCoord.Value);
						if (IsValidFigurePosition(figurePosition))
						{
							return new FigureMove(figurePosition);
						}
					}

					break;
				}
				case 3:
				{
					if (moveString.ToLower() == "cap")
						return new Capitulation();

					var xCoord      = GetXCoord     (moveString[0]);
					var yCoord      = GetYCoord     (moveString[1]);
					var orientation = GetOrientation(moveString[2]);

					if (xCoord.HasValue && yCoord.HasValue && orientation.HasValue)
					{
						var wallPosition = new FieldCoordinate(xCoord.Value, yCoord.Value);

						if (IsValidWallPosition(wallPosition) &&
							IsValidWallOrientation(orientation.Value))
						{
							return new WallMove(new Wall(wallPosition, orientation.Value));
						}							
					}
						
					break;
				}				
			}

			return null;
		}

		private static bool IsValidFigurePosition(FieldCoordinate coord)
		{
			return coord.XCoord >= XField.A &&
			       coord.XCoord <= XField.I &&
				   coord.YCoord >= YField.Nine &&
			       coord.YCoord <= YField.One;
		}

		private static bool IsValidWallPosition(FieldCoordinate coord)
		{
			return coord.XCoord >= XField.A &&
			       coord.XCoord <= XField.H &&
			       coord.YCoord >= YField.Nine &&
			       coord.YCoord <= YField.Two;
		}

		private static bool IsValidWallOrientation(WallOrientation orientation)
		{
			return orientation == WallOrientation.Horizontal ||
				   orientation == WallOrientation.Vertical;
		}

		private static WallOrientation? GetOrientation (char s)
		{
			switch (char.ToLower(s))
			{
				case 'h': return WallOrientation.Horizontal;
				case 'v': return WallOrientation.Vertical;
			}

			return null;
		}

		private static XField? GetXCoord (char s)
		{
			switch (char.ToLower(s))
			{
				case 'a': return XField.A;
				case 'b': return XField.B;
				case 'c': return XField.C;
				case 'd': return XField.D;
				case 'e': return XField.E;
				case 'f': return XField.F;
				case 'g': return XField.G;
				case 'h': return XField.H;
				case 'i': return XField.I;
			}

			return null;
		}

		private static YField? GetYCoord (char s)
		{
			switch (s)
			{
				case '1': return YField.One;
				case '2': return YField.Two;
				case '3': return YField.Three;
				case '4': return YField.Four;
				case '5': return YField.Five;
				case '6': return YField.Six;
				case '7': return YField.Seven;
				case '8': return YField.Eight;
				case '9': return YField.Nine;
			}

			return null;
		}
	}
}

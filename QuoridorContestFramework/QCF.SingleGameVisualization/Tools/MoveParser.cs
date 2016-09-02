using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;
using QCF.Contest.Contracts.Moves;

namespace QCF.SingleGameVisualization.Tools
{
	internal static class MoveParser
	{
		public static Move GetMove(string s, BoardState stateBeforeMove, Player playerAtMove)
		{
			var moveString = s.Trim();

			switch (moveString.Length)
			{
				case 2:
				{
					var xCoord = GetXCoord(moveString[0]);
					var yCoord = GetYCoord(moveString[1]);

					if (xCoord.HasValue && yCoord.HasValue)
						return new FigureMove(stateBeforeMove, 
											  playerAtMove, 											  
											  new FieldCoordinate(xCoord.Value, yCoord.Value));

					break;
				}
				case 3:
				{
					var xCoord      = GetXCoord     (moveString[0]);
					var yCoord      = GetYCoord     (moveString[1]);
					var orientation = GetOrientation(moveString[2]);

					if (xCoord.HasValue && yCoord.HasValue && orientation.HasValue)
						if (xCoord.Value < XField.I && yCoord.Value < YField.One)
							return new WallMove(stateBeforeMove,
												playerAtMove,
												new Wall(new FieldCoordinate(xCoord.Value, yCoord.Value),orientation.Value));

					break;
				}
			}

			return null;
		}

		private static WallOrientation? GetOrientation(char s)
		{
			switch (char.ToLower(s))
			{
				case 'h': return WallOrientation.Horizontal;
				case 'v': return WallOrientation.Vertical;
			}

			return null;
		}

		private static XField? GetXCoord(char s)
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

		private static YField? GetYCoord(char s)
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

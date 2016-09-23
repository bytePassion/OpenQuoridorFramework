namespace OQF.Bot.Contracts.Coordination
{
	/// <summary>
	/// This struct represents a Position on the Quoridor-board
	/// Note: There are 81 legal positions on the board
	/// </summary>

	public struct FieldCoordinate
	{
		private readonly int hashCode;

		public FieldCoordinate(XField xCoord, YField yCoord)
		{
			XCoord = xCoord;
			YCoord = yCoord;

			hashCode = new {xCoord, yCoord}.GetHashCode();
		}

		public XField XCoord { get; }
		public YField YCoord { get; }

		public override string ToString()
		{
			return XCoordToString(XCoord) + YCoordToString(YCoord);
		}

		public override bool Equals(object obj)
		{			
			if (GetType() != obj.GetType())
				return false;

			var objAsFieldCoord = (FieldCoordinate) obj;

			return XCoord == objAsFieldCoord.XCoord && YCoord == objAsFieldCoord.YCoord;
		}

		public override int GetHashCode()
		{
			return hashCode;
		}

		public static bool operator ==(FieldCoordinate c1, FieldCoordinate c2) =>  c1.Equals(c2);
		public static bool operator !=(FieldCoordinate c1, FieldCoordinate c2) => !c1.Equals(c2);		

	    private static string XCoordToString(XField xCoord)
		{			
			switch (xCoord)
			{
				case XField.A: return "a";
				case XField.B: return "b";
				case XField.C: return "c";
				case XField.D: return "d";
				case XField.E: return "e";
				case XField.F: return "f";
				case XField.G: return "g";
				case XField.H: return "h";
				case XField.I: return "i";
			}

			return "error";
		}

		private static string YCoordToString(YField yCoord)
		{
			switch (yCoord)
			{
				case YField.One:   return "1";
				case YField.Two:   return "2";
				case YField.Three: return "3";
				case YField.Four:  return "4";
				case YField.Five:  return "5";
				case YField.Six:   return "6";
				case YField.Seven: return "7";
				case YField.Eight: return "8";
				case YField.Nine:  return "9";
			}

			return "error";
		}
	}
}

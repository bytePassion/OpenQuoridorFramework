using OQF.Bot.Contracts.Coordination;

namespace SimpleWalkingBot.Graph
{
	internal static class FieldCoordinateExtensions
	{
		public static bool ExistsLeft   (this FieldCoordinate coord) => coord.XCoord != XField.A;
		public static bool ExistsRight  (this FieldCoordinate coord) => coord.XCoord != XField.I;
		public static bool ExistsTop    (this FieldCoordinate coord) => coord.YCoord != YField.Nine;
		public static bool ExistsBottom (this FieldCoordinate coord) => coord.YCoord != YField.One;

		public static FieldCoordinate GetLeft   (this FieldCoordinate coord) => new FieldCoordinate(coord.XCoord - 1, coord.YCoord    ); 
		public static FieldCoordinate GetRight  (this FieldCoordinate coord) => new FieldCoordinate(coord.XCoord + 1, coord.YCoord    );
		public static FieldCoordinate GetTop    (this FieldCoordinate coord) => new FieldCoordinate(coord.XCoord,     coord.YCoord - 1);
		public static FieldCoordinate GetBottom (this FieldCoordinate coord) => new FieldCoordinate(coord.XCoord,     coord.YCoord + 1);
	}
}

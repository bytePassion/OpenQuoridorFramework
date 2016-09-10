using System.Windows.Media;

namespace OQF.HumanVsPlayer.Global
{
	internal static class Constants
	{
		public static readonly SolidColorBrush TopPlayerActiveColor     = new SolidColorBrush(Colors.Red);
		public static readonly SolidColorBrush BottomPlayerActiveColor  = new SolidColorBrush(Colors.GreenYellow);
		public static readonly SolidColorBrush PlayerInactiveColor      = new SolidColorBrush(Colors.DarkGray);
		public static readonly SolidColorBrush FieldBackgroundColor     = new SolidColorBrush(Colors.Aqua);
		public static readonly SolidColorBrush FieldColor               = new SolidColorBrush(Colors.White);
		public static readonly SolidColorBrush FrameColor		        = new SolidColorBrush(Colors.Black);
		public static readonly SolidColorBrush WallColor                = new SolidColorBrush(Colors.DeepPink);
		public static readonly SolidColorBrush PossibleMoveFieldColor   = new SolidColorBrush(Colors.Orange);
		public static readonly SolidColorBrush PotentialPlacedWallColor = new SolidColorBrush(Colors.Black);
	}
}

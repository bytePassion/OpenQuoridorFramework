using System.Windows;
using System.Windows.Media;

namespace OQF.PlayerVsBot.Global
{
	internal static class Constants
	{
		//public static readonly SolidColorBrush TopPlayerActiveColor     = new SolidColorBrush(Colors.Red);
		//public static readonly SolidColorBrush BottomPlayerActiveColor  = new SolidColorBrush(Colors.GreenYellow);
		public static readonly SolidColorBrush PlayerInactiveColor      = new SolidColorBrush(Colors.DarkGray);
		public static readonly SolidColorBrush FieldBackgroundColor     = new SolidColorBrush(Color.FromRgb(121, 93, 86));
		public static readonly SolidColorBrush FieldColor               = new SolidColorBrush(Color.FromRgb(97, 64, 56));
		public static readonly SolidColorBrush FrameColor		        = new SolidColorBrush(Colors.Black);
		public static readonly SolidColorBrush WallColor                = new SolidColorBrush(Colors.AntiqueWhite);
		public static readonly SolidColorBrush PossibleMoveFieldColor   = new SolidColorBrush(Color.FromArgb(255, 255, 206, 0));
		public static readonly SolidColorBrush PotentialPlacedWallColor = new SolidColorBrush(Color.FromArgb(207, 255, 255, 255));

	    public static readonly Brush TopPlayerActiveColor = new LinearGradientBrush()
	    {
	        MappingMode = BrushMappingMode.RelativeToBoundingBox,
	        EndPoint = new Point(0.5, 1),
	        StartPoint = new Point(0.5, 0),
	        GradientStops = new GradientStopCollection()
	        {
	            new GradientStop(Color.FromRgb(241, 241, 241), 0.159),
	            new GradientStop(Color.FromRgb(178, 178, 178), 0.674)
	        }
	    };

        public static readonly Brush BottomPlayerActiveColor = new LinearGradientBrush()
        {
            MappingMode = BrushMappingMode.RelativeToBoundingBox,
            EndPoint = new Point(0.5, 1),
            StartPoint = new Point(0.5, 0),
            GradientStops = new GradientStopCollection()
            {
                new GradientStop(Color.FromRgb(122, 122, 122), 0.159),
                new GradientStop(Color.FromRgb(48, 48, 48), 0.674)
            }
        };

        public static readonly Size SizeFallBackValue = new Size(300,300);
	}
}

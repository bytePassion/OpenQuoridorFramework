using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Lib.SemanicTypes;
using Point = Lib.SemanicTypes.Point;

namespace OQF.PlayerVsBot.Visualization.Behaviors
{
	public class ReportRelativeMousePositionBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty MousePositionProperty = 
			DependencyProperty.Register(nameof(MousePosition), 
										typeof(Point), 
										typeof(ReportRelativeMousePositionBehavior));

		public Point MousePosition
		{
			get { return (Point) GetValue(MousePositionProperty); }
			set { SetValue(MousePositionProperty, value); }
		}

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.MouseMove  += OnMouseMove;
			AssociatedObject.MouseLeave += OnMouseLeave;
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.MouseMove  -= OnMouseMove;
			AssociatedObject.MouseLeave -= OnMouseLeave;
		}

		private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			MousePosition = new Point(new XCoord(-1), new YCoord(-1));
		}
		
		private void OnMouseMove (object sender, MouseEventArgs mouseEventArgs)
		{
			var position = mouseEventArgs.GetPosition(AssociatedObject);
			MousePosition = new Point(new XCoord(position.X), new YCoord(position.Y));
		}		
	}
}
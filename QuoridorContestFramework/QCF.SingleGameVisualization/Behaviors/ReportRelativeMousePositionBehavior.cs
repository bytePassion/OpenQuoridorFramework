using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using QCF.Tools.SemanticTypes;
using Point = QCF.Tools.SemanticTypes.Point;

namespace QCF.SingleGameVisualization.Behaviors
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
			AssociatedObject.MouseMove += OnMouseMove;
		}		

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.MouseMove -= OnMouseMove;
		}

		private void OnMouseMove (object sender, MouseEventArgs mouseEventArgs)
		{
			var position = mouseEventArgs.GetPosition(AssociatedObject);
			MousePosition = new Point(new XCoord(position.X), new YCoord(position.Y));
		}		
	}
}
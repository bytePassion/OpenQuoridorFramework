using System.Windows;
using System.Windows.Interactivity;

namespace QCF.SingleGameVisualization.Behaviors
{
	public class SizeReportingBehavior : Behavior<FrameworkElement>
	{
		
		public static readonly DependencyProperty ReportedSizeProperty = 
			DependencyProperty.Register(nameof(ReportedSize), 
										typeof (Size), 
										typeof (SizeReportingBehavior));

		public Size ReportedSize
		{
			get { return (Size) GetValue(ReportedSizeProperty); }
			set { SetValue(ReportedSizeProperty, value); }
		}
				
		protected override void OnAttached()
		{
			base.OnAttached();			
			AssociatedObject.SizeChanged += OnSizeChanged;	
			AssociatedObject.Loaded      += OnLoaded;
		}		

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.SizeChanged -= OnSizeChanged;
			AssociatedObject.Loaded      -= OnLoaded;
		}

		private void OnLoaded (object sender, RoutedEventArgs routedEventArgs)
		{
			ReportSize((UIElement) sender);
		}

		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			ReportSize((UIElement)sender);
		}

		private void ReportSize(UIElement sender)
		{
			ReportedSize = sender.RenderSize;			
		}
	}
}

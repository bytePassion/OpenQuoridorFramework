using System.Windows;
using System.Windows.Interactivity;

namespace Lib.Wpf.Behaviors
{
	// TODO: change to semantic-types-size

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
			ReportSize((FrameworkElement) sender);
		}

		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			ReportSize((FrameworkElement)sender);
		}

		private void ReportSize(FrameworkElement sender)
		{
			ReportedSize = sender.RenderSize;
		}
	}
}

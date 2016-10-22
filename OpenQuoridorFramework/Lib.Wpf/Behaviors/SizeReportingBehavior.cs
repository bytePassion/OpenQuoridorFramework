using System.Windows;
using System.Windows.Interactivity;
using Lib.SemanicTypes;

namespace Lib.Wpf.Behaviors
{
	public class SizeReportingBehavior : Behavior<FrameworkElement>
	{
		
		public static readonly DependencyProperty ReportedSizeProperty = 
			DependencyProperty.Register(nameof(ReportedSize), 
										typeof (SemanicTypes.Size), 
										typeof (SizeReportingBehavior));

		public SemanicTypes.Size ReportedSize
		{
			get { return (SemanicTypes.Size) GetValue(ReportedSizeProperty); }
			set { SetCurrentValue(ReportedSizeProperty, value); }
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
			ReportedSize = new SemanicTypes.Size(new Width(sender.RenderSize.Width), 
												 new Height(sender.RenderSize.Height)); 
		}
	}
}

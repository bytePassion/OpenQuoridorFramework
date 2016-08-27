using System.Windows;
using System.Windows.Interactivity;
using QCF.UiTools.Utils;

namespace QCF.SingleGameVisualization.Behaviors
{
	internal class KeepQuadratic : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.SizeChanged += OnSizeChanged;
		}		

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			var currentSize = sizeChangedEventArgs.NewSize;

			if (!GeometryLibUtils.DoubleEquals(currentSize.Height, currentSize.Width))
			{
				var newSize = currentSize.Height < currentSize.Width 
									? new Size(currentSize.Height, currentSize.Height) 
									: new Size(currentSize.Width,  currentSize.Width);

				((FrameworkElement) sender).Height = newSize.Height;
				((FrameworkElement) sender).Width  = newSize.Width;
			}
		}
	}
}

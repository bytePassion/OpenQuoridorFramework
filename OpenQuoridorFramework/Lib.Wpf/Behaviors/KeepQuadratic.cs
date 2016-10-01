using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Lib.Wpf.Behaviors
{
	public class KeepQuadratic : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			var border = (Border) AssociatedObject.Parent;

			border.SizeChanged += OnSizeChanged;			
		}		
		
		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			var parentSize = sizeChangedEventArgs.NewSize;

			var lengthOfSmalerSide = parentSize.Height > parentSize.Width 
										? parentSize.Width 
										: parentSize.Height;

			AssociatedObject.Height = lengthOfSmalerSide - 32;
			AssociatedObject.Width  = lengthOfSmalerSide - 32;
		}
	}
}

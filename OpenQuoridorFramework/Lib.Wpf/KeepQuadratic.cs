using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Lib.Wpf.Behaviors
{
	public class KeepQuadratic : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty MarginProperty = 
			DependencyProperty.Register("Margin", 
									    typeof(double), 
										typeof(KeepQuadratic), 
										new PropertyMetadata(0.0));

		public double Margin
		{
			get { return (double) GetValue(MarginProperty); }
			set { SetValue(MarginProperty, value); }
		}

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

			AssociatedObject.Height = lengthOfSmalerSide - Margin * 2;
			AssociatedObject.Width  = lengthOfSmalerSide - Margin * 2;
		}
	}
}

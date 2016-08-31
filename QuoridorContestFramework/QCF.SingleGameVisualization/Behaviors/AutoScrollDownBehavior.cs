using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace QCF.SingleGameVisualization.Behaviors
{
	internal class AutoScrollDownBehavior : Behavior<ScrollViewer>
	{
		public static readonly DependencyProperty IsAutoScrollActiveProperty = 
			DependencyProperty.Register(nameof(IsAutoScrollActive), 
										typeof(bool), 
										typeof(AutoScrollDownBehavior));

		public bool IsAutoScrollActive
		{
			get { return (bool) GetValue(IsAutoScrollActiveProperty); }
			set { SetValue(IsAutoScrollActiveProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.LayoutUpdated += OnLayoutUpdated;			
		}		

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
		}

		private void OnLayoutUpdated (object sender, EventArgs eventArgs)
		{
			if (IsAutoScrollActive)
				AssociatedObject.ScrollToEnd();
		}
	}
}

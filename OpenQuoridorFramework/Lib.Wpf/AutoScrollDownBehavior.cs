using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Lib.Wpf.Behaviors
{
	public class AutoScrollDownBehavior : Behavior<ScrollViewer>
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

            AssociatedObject.ScrollChanged += OnScrollChanged;
        }        

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.ScrollChanged -= OnScrollChanged;
        }

		private void OnScrollChanged (object sender, ScrollChangedEventArgs e)
		{
			var sv = sender as ScrollViewer;
			if (IsAutoScrollActive)
			{
				var autoScrollToEnd = true;
				if (sv.Tag != null)
				{
					autoScrollToEnd = (bool)sv.Tag;
				}
				if (e.ExtentHeightChange == 0) // user scroll
				{
					autoScrollToEnd = sv.ScrollableHeight == sv.VerticalOffset;
				}
				else
				{
					if (autoScrollToEnd)
					{
						sv.ScrollToEnd();
					}
				}
				sv.Tag = autoScrollToEnd;
			}
		}
	}
}
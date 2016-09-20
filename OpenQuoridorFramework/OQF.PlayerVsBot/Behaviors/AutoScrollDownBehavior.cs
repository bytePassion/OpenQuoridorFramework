using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace OQF.PlayerVsBot.Behaviors
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

            AssociatedObject.ScrollChanged += OnScrollChanged;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if (IsAutoScrollActive)
            {
                bool AutoScrollToEnd = true;
                if (sv.Tag != null)
                {
                    AutoScrollToEnd = (bool) sv.Tag;
                }
                if (e.ExtentHeightChange == 0) // user scroll
                {
                    AutoScrollToEnd = sv.ScrollableHeight == sv.VerticalOffset;
                }
                else
                {
                    if (AutoScrollToEnd)
                    {
                        sv.ScrollToEnd();
                    }
                }
                sv.Tag = AutoScrollToEnd;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.ScrollChanged -= OnScrollChanged;
        }

    }
}
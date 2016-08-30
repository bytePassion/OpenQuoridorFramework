using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace QCF.SingleGameVisualization.Behaviors
{
	internal class ListBoxAutoScrollDownBehavior : Behavior<ListBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Items.CurrentChanged += ItemsOnCurrentChanged;			
		}
		
		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.Items.CurrentChanged -= ItemsOnCurrentChanged;
		}

		private void ItemsOnCurrentChanged (object sender, EventArgs eventArgs)
		{
			if (AssociatedObject.Items?.Count > 0)
				AssociatedObject.ScrollIntoView(AssociatedObject.Items[AssociatedObject.Items.Count - 1]);
		}
	}
}

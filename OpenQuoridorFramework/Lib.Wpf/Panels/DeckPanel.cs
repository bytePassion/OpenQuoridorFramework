using System.Windows;
using System.Windows.Controls;

namespace Lib.Wpf.Panels
{
	public class DeckPanel : Panel
    {		
	    public static readonly DependencyProperty SelectedLayerProperty = 
			DependencyProperty.Register(nameof(SelectedLayer), 
										typeof(int), 
										typeof(DeckPanel),
										new FrameworkPropertyMetadata(0,FrameworkPropertyMetadataOptions.AffectsMeasure));
		
		public int SelectedLayer
	    {
		    get { return (int) GetValue(SelectedLayerProperty); }
		    set { SetValue(SelectedLayerProperty, value); }
	    }
				
		protected override Size MeasureOverride (Size availableSize)
		{
			if (Children == null || Children.Count == 0)
				return availableSize;

			var visibleChild = Children[SelectedLayer];
			visibleChild.Measure(availableSize);

			return availableSize;
		}

		protected override Size ArrangeOverride (Size finalSize)
		{
			if (Children == null || Children.Count == 0)
				return finalSize;

			var visibleChild = Children[SelectedLayer];
					
			visibleChild.Arrange(new Rect(new Point(0,0), finalSize));


			int i = 0;
			foreach (var child in Children)
			{
				SetZIndex((UIElement) child, ReferenceEquals(child, visibleChild) ? Children.Count+2 : i++);
			}

			return finalSize;
		}		
	}
}



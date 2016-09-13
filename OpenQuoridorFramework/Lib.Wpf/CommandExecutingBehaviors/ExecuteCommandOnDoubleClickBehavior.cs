using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Lib.Wpf.CommandExecutingBehaviors
{
	public class ExecuteCommandOnDoubleClickBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty CommandProperty 
			= DependencyProperty.Register(nameof(Command), 
										  typeof (ICommand), 
										  typeof (ExecuteCommandOnDoubleClickBehavior));

		public static readonly DependencyProperty CommandParameterProperty 
			= DependencyProperty.Register(nameof(CommandParameter), 
										  typeof (object), 
										  typeof (ExecuteCommandOnDoubleClickBehavior));
		
		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.MouseLeftButtonDown += OnMouseDown;
			AssociatedObject.MouseLeave          += OnMouseLeave;
			AssociatedObject.MouseLeftButtonUp   += AssociatedObjectOnMouseLeftButtonUp;			
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
			AssociatedObject.MouseLeave          -= OnMouseLeave;
			AssociatedObject.MouseLeftButtonUp   -= AssociatedObjectOnMouseLeftButtonUp;
		}

		private bool possibleExecution = false;
		private int clickCount = 0;

		private void AssociatedObjectOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (possibleExecution && clickCount > 0 && clickCount % 2 == 0)
			{
				possibleExecution = false;

				if (Command != null) 
					if(Command.CanExecute(CommandParameter))
						Command.Execute(CommandParameter);
			}
		}

		private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			possibleExecution = false;
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			clickCount = mouseButtonEventArgs.ClickCount;
			possibleExecution = true;
		}		
	}
}

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Lib.Wpf.CommandExecutingBehaviors
{
	public class ExecuteCommandOnClickBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty CommandProperty 
			= DependencyProperty.Register(nameof(Command), 
										  typeof (ICommand), 
										  typeof (ExecuteCommandOnClickBehavior));
		
		public static readonly DependencyProperty CommandParameterProperty 
			= DependencyProperty.Register(nameof(CommandParameter), 
										  typeof (object), 
										  typeof (ExecuteCommandOnClickBehavior));

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

		private void AssociatedObjectOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (possibleExecution)
			{
				possibleExecution = false;

				if (Command != null)
					if (Command.CanExecute(CommandParameter))
						Command.Execute(CommandParameter);
			}
		}

		private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			possibleExecution = false;
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			possibleExecution = true;
		}		
	}
}

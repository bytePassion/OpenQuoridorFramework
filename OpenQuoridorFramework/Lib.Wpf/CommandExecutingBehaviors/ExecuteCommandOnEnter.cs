using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Lib.Wpf.CommandExecutingBehaviors
{
	public class ExecuteCommandOnEnter : Behavior<FrameworkElement>
    {

	    public static readonly DependencyProperty CommandProperty = 
			DependencyProperty.Register(nameof(Command), 
										typeof (ICommand), 
										typeof (ExecuteCommandOnEnter));

	    public static readonly DependencyProperty CommandParameterProperty = 
			DependencyProperty.Register(nameof(CommandParameter), 
										typeof (object), 
										typeof (ExecuteCommandOnEnter));

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

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.KeyDown += OnKeyDown;
		}

		protected override void OnDetaching ()
		{
			base.OnAttached();
			AssociatedObject.KeyDown -= OnKeyDown;
		}

	    private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
	    {		    
		    if (keyEventArgs.Key == Key.Enter)
				if (Command != null)
					if (Command.CanExecute(CommandParameter))
						Command.Execute(CommandParameter);
	    }	    
    }
}

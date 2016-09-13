using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Lib.Wpf.CommandExecutingBehaviors
{
	public class ExecuteCommandWhenToggleButtonGetsCheckedBehavior : Behavior<ToggleButton>
	{

		public static readonly DependencyProperty CommandProperty 
			= DependencyProperty.Register(nameof(Command),
										  typeof(ICommand),
										  typeof(ExecuteCommandWhenToggleButtonGetsCheckedBehavior));

		public static readonly DependencyProperty CommandParameterProperty 
			= DependencyProperty.Register(nameof(CommandParameter),
										  typeof(object),
										  typeof(ExecuteCommandWhenToggleButtonGetsCheckedBehavior));

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.Checked += OnButtonChecked;			
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.Checked -= OnButtonChecked;
		}

		private void OnButtonChecked(object sender, RoutedEventArgs e)
		{
			if (Command != null)
				if (Command.CanExecute(CommandParameter))
					Command.Execute(CommandParameter);
		}		
	}
}

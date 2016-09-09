using System.Windows.Input;
using WpfLib.ViewModelBase;

namespace OQF.SingleGameVisualization.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
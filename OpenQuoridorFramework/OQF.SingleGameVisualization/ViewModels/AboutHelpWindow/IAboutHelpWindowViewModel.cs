using System.Windows.Input;
using OQF.Tools.WpfTools.ViewModelBase;

namespace OQF.SingleGameVisualization.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
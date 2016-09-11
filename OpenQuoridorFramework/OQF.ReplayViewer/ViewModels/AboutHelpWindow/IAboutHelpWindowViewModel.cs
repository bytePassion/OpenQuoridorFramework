using System.Windows.Input;
using WpfLib.ViewModelBase;

namespace OQF.ReplayViewer.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
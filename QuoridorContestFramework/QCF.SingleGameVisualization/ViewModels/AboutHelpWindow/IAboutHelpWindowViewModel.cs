using System.Windows.Input;
using QCF.Tools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
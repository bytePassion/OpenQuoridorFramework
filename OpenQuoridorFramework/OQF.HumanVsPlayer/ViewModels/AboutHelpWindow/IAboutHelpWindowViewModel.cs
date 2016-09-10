using System.Windows.Input;
using WpfLib.ViewModelBase;

namespace OQF.HumanVsPlayer.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
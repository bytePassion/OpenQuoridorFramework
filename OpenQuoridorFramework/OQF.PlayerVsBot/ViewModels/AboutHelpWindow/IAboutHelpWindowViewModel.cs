using System.Windows.Input;
using WpfLib.ViewModelBase;

namespace OQF.PlayerVsBot.ViewModels.AboutHelpWindow
{
	internal interface IAboutHelpWindowViewModel : IViewModel
	{
		ICommand Close { get; }
	}
}
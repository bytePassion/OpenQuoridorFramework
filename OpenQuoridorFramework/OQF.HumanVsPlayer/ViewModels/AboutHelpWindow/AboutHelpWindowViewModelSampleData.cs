using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace OQF.HumanVsPlayer.ViewModels.AboutHelpWindow
{
	internal class AboutHelpWindowViewModelSampleData : IAboutHelpWindowViewModel
	{
		public ICommand Close => null;

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;				
	}
}
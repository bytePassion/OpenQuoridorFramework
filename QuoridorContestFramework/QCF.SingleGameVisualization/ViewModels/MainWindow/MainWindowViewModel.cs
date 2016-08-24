using System.ComponentModel;
using QCF.UiTools.WpfTools.ViewModelBase;

namespace QCF.SingleGameVisualization.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
using System.Windows.Input;
using Lib.Wpf.ViewModelBase;

namespace ProgressCodingTest.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
	{
		ICommand ConvertProgressToString { get; }
		ICommand ConvertStringToProgress { get; }

		string Progress { get; set; }
		string ProgressAsString { get; set; }
	}
}
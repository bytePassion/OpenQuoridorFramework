using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using OQF.CommonUiElements.Dialogs.StringInput.ViewModel;

namespace OQF.CommonUiElements.Dialogs.StringInput
{
	public static class StringInputDialogService
	{
		public static async Task<string> Show (string promt)
		{			
			var stringInputDialogViewModel = new StringInputDialogViewModel(promt);

			var inputDialog = new StringInputDialog()
			{
				DataContext = stringInputDialogViewModel
			};

			var result = (string) await DialogHost.Show(inputDialog, "RootDialog");

			stringInputDialogViewModel.Dispose();

			return result;
		}
	}
}

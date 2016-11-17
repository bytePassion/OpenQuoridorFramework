using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using OQF.CommonUiElements.Dialogs.YesNo.ViewModel;

namespace OQF.CommonUiElements.Dialogs.YesNo
{
	public static class YesNoDialogService
	{
		public static async Task<bool> Show (string message)
		{
			var yesNoDialogViewModel = new YesNoDialogViewModel(message);

			var yesNoDialog = new YesNoDialog
			{
				DataContext = yesNoDialogViewModel
			};

			var dialogResult = (bool) await DialogHost.Show(yesNoDialog, "RootDialog");

			yesNoDialogViewModel.Dispose();			

			return dialogResult;
		}
	}
}

using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using OQF.CommonUiElements.Dialogs.Notification.ViewModel;

namespace OQF.CommonUiElements.Dialogs.Notification
{
	public static class NotificationDialogService
	{
		public static async Task Show(string message, string buttonCaption)
		{
			try
			{
				var winningDialogViewModel = new NotificationDialogViewModel(message, buttonCaption);

				var view = new NotificationDialog
				{
					DataContext = winningDialogViewModel
				};

				await DialogHost.Show(view, "RootDialog");

				winningDialogViewModel.Dispose();
			}
			catch
			{
				MessageBox.Show(message);
			}			
		}
	}
}

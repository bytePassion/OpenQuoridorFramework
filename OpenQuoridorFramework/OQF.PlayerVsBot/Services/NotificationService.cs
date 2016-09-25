using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using OQF.PlayerVsBot.ViewModels.NotificationDialog;

namespace OQF.PlayerVsBot.Services
{
	internal static class NotificationService
	{
		public static async Task Show(string message, string buttonCaption)
		{
			var winningDialogViewModel = new NotificationDialogViewModel(message, buttonCaption);

			var view = new Views.NotificationDialog
			{
				DataContext = winningDialogViewModel
			};

			await DialogHost.Show(view, "RootDialog");
			
			winningDialogViewModel.Dispose();
		}
	}
}

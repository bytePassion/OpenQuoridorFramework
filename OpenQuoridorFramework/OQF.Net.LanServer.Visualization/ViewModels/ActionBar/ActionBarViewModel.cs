using System.ComponentModel;
using System.Windows.Input;
using Lib.FrameworkExtension;
using Lib.Wpf.Commands;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Info;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.Resources;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.Net.LanServer.Visualization.ViewModels.ActionBar
{
	public class ActionBarViewModel : ViewModel, IActionBarViewModel
	{
		public ActionBarViewModel(ILanguageSelectionViewModel languageSelectionViewModel)
		{
			CultureManager.CultureChanged += RefreshCaptions;

			LanguageSelectionViewModel = languageSelectionViewModel;

			ShowAboutHelp = new Command(DoShowAboutHelp);
		}		

		public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }

		public ICommand ShowAboutHelp { get; }
		
		private void DoShowAboutHelp ()
		{
			InfoWindowService.Show(OpenQuoridorFrameworkInfo.Applications.NetworkLanServer.Info,
								   //InfoPage.LanServerApplicationInfo,								  
								   InfoPage.About);
		}

		public string OpenInfoButtonToolTipCaption => Captions.PvB_OpenInfoButtonToolTipCaption;

		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(OpenInfoButtonToolTipCaption));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}

		public override event PropertyChangedEventHandler PropertyChanged;				
	}
}

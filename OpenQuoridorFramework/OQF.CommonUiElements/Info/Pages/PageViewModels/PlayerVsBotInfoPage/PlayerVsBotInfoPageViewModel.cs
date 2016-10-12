using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.CommonUiElements.Language;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.PlayerVsBotInfoPage
{
	internal class PlayerVsBotInfoPageViewModel : ViewModel, IPlayerVsBotInfoPageViewModel
	{
		public PlayerVsBotInfoPageViewModel()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}
		
		public string PageHeader    => Captions.PvBH_PageHeader;
		public string Note          => Captions.PvBH_Note;

		public string Explanation01 => Captions.PvBH_Explanation01;
		public string Explanation02 => Captions.PvBH_Explanation02;
		public string Explanation03 => Captions.PvBH_Explanation03;
		public string Explanation04 => Captions.PvBH_Explanation04;
		public string Explanation05 => Captions.PvBH_Explanation05;
		public string Explanation06 => Captions.PvBH_Explanation06;
		public string Explanation07 => Captions.PvBH_Explanation07;
		public string Explanation08 => Captions.PvBH_Explanation08;
		public string Explanation09 => Captions.PvBH_Explanation09;
		public string Explanation10 => Captions.PvBH_Explanation10;
		public string Explanation11 => Captions.PvBH_Explanation11;
		public string Explanation12 => Captions.PvBH_Explanation12;
		public string Explanation13 => Captions.PvBH_Explanation13;
		public string Explanation14 => Captions.PvBH_Explanation14;
		public string Explanation15 => Captions.PvBH_Explanation15;
		public string Explanation16 => Captions.PvBH_Explanation16;
		public string Explanation17 => Captions.PvBH_Explanation17;
		public string Explanation18 => Captions.PvBH_Explanation18;
		public string Explanation19 => Captions.PvBH_Explanation19;
		public string Explanation20 => Captions.PvBH_Explanation20;
		public string Explanation21 => Captions.PvBH_Explanation21;
        public string DisplayName   => Captions.IP_AboutButtonCaption;

        private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),
										 nameof(Note),
										 nameof(Explanation01),
										 nameof(Explanation02),
										 nameof(Explanation03),
										 nameof(Explanation04),
										 nameof(Explanation05),
										 nameof(Explanation06),
										 nameof(Explanation07),
										 nameof(Explanation08),
										 nameof(Explanation09),
										 nameof(Explanation10),
										 nameof(Explanation11),
										 nameof(Explanation12),
										 nameof(Explanation13),
										 nameof(Explanation14),
										 nameof(Explanation15),
										 nameof(Explanation16),
										 nameof(Explanation17),
										 nameof(Explanation18),
										 nameof(Explanation19),
										 nameof(Explanation20),
										 nameof(Explanation21),
										 nameof(DisplayName));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}

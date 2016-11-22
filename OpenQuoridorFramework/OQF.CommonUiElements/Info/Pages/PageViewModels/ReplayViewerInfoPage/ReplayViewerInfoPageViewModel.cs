using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.ReplayViewerInfoPage
{
	internal class ReplayViewerInfoPageViewModel : ViewModel , IReplayViewerInfoPageViewModel 
	{
		public ReplayViewerInfoPageViewModel()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}

		public string DisplayName   => Captions.IP_ReplayViewerInfoButtonCaption;

		public string PageHeader    => Captions.RVH_PageHeader;
		public string Explanation01 => Captions.RVH_Explanation01;
		public string Explanation02 => Captions.RVH_Explanation02;
		public string Explanation03 => Captions.RVH_Explanation03;
		public string Explanation04 => Captions.RVH_Explanation04;
		public string Explanation05 => Captions.RVH_Explanation05;
		public string Explanation06 => Captions.RVH_Explanation06;
		public string Explanation07 => Captions.RVH_Explanation07;
		public string Explanation08 => Captions.RVH_Explanation08;
		public string Explanation09 => Captions.RVH_Explanation09;
		public string Explanation10 => Captions.RVH_Explanation10;


		private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),										 
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
										 nameof(DisplayName));
		}

		protected override void CleanUp ()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

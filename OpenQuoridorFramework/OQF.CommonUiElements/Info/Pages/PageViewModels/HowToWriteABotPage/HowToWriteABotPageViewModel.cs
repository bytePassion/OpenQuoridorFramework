using System.ComponentModel;
using Lib.FrameworkExtension;
using Lib.Wpf.ViewModelBase;
using OQF.Resources.LanguageDictionaries;
using OQF.Utils;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.HowToWriteABotPage
{
	internal class HowToWriteABotPageViewModel : ViewModel, IHowToWriteABotPageViewModel
	{
		public HowToWriteABotPageViewModel ()
		{
			CultureManager.CultureChanged += RefreshCaptions;
		}		

		public string PageHeader => Captions.HTWAB_PageHeader;

		public string GeneralProceedingParagraphHeader => Captions.HTWAB_GeneralProceedingParagraphHeader;
		public string GeneralProceedingItem1           => Captions.HTWAB_GeneralProceedingItem1;
		public string GeneralProceedingItem2           => Captions.HTWAB_GeneralProceedingItem2;
		public string GeneralProceedingItem3           => Captions.HTWAB_GeneralProceedingItem3;
		public string GeneralProceedingItem4           => Captions.HTWAB_GeneralProceedingItem4;
		public string GeneralProceedingItem5           => Captions.HTWAB_GeneralProceedingItem5;

		public string ImportantClassesParagraphHeader => Captions.HTWAB_ImportantClassesParagraphHeader;
		public string ImportantClassesParagraphNote   => Captions.HTWAB_ImportantClassesParagraphNote;
		public string FieldCoordinateParagraphHeader  => Captions.HTWAB_FieldCoordinateParagraphHeader;
		public string FieldCoordinateParagraphText    => Captions.HTWAB_FieldCoordinateParagraphText;
		public string PlayerAndStateParagraphHeader   => Captions.HTWAB_PlayerAndStateParagraphHeader;
		public string PlayerAndStateParagraphText     => Captions.HTWAB_PlayerAndStateParagraphText;
		public string WallParagraphHeader             => Captions.HTWAB_WallParagraphHeader;
		public string WallParagraphText               => Captions.HTWAB_WallParagraphText;
		public string BoardStateParagraphHeader       => Captions.HTWAB_BoardStateParagraphHeader;
		public string BoardStateParagraphText         => Captions.HTWAB_BoardStateParagraphText;
		public string MovesParagraphHeader            => Captions.HTWAB_MovesParagraphHeader;
		public string MovesParagraphText              => Captions.HTWAB_MovesParagraphText;
		public string GameConstraintsParagraphHeader  => Captions.HTWAB_GameConstraintsParagraphHeader;
		public string GameConstraintsParagraphText    => Captions.HTWAB_GameConstraintsParagraphText;

		public string GameFlowParagraphHeader => Captions.HTWAB_GameFlowParagraphHeader;
		public string GameFlowItem1           => Captions.HTWAB_GameFlowItem1;
		public string GameFlowItem2           => Captions.HTWAB_GameFlowItem2;
		public string GameFlowItem3           => Captions.HTWAB_GameFlowItem3;
		public string GameFlowItem4           => Captions.HTWAB_GameFlowItem4;
		public string GameFlowItem5           => Captions.HTWAB_GameFlowItem5;

		public string ExampleParagraphHeader => Captions.HTWAB_ExampleParagraphHeader;
		public string ExampleParagraphText   => Captions.HTWAB_ExampleParagraphText;

		public string HintParagrapHeader => Captions.HTWAB_HintParagraphHeader;
		public string HintParagrapText   => Captions.HTWAB_HintParagraphText;
        public string DisplayName        => Captions.IP_HowToWriteABotButtonCaption;


        private void RefreshCaptions ()
		{
			PropertyChanged.Notify(this, nameof(PageHeader),
										 nameof(GeneralProceedingParagraphHeader),
										 nameof(GeneralProceedingItem1),
										 nameof(GeneralProceedingItem2),
										 nameof(GeneralProceedingItem3),
										 nameof(GeneralProceedingItem4),
										 nameof(GeneralProceedingItem5),
										 nameof(ImportantClassesParagraphHeader),
										 nameof(ImportantClassesParagraphNote),
										 nameof(FieldCoordinateParagraphHeader),
										 nameof(FieldCoordinateParagraphText),
										 nameof(PlayerAndStateParagraphHeader),
										 nameof(PlayerAndStateParagraphText),
										 nameof(WallParagraphHeader),
										 nameof(WallParagraphText),
										 nameof(BoardStateParagraphHeader),
										 nameof(BoardStateParagraphText),
										 nameof(MovesParagraphHeader),
										 nameof(MovesParagraphText),
										 nameof(GameConstraintsParagraphHeader),
										 nameof(GameConstraintsParagraphText),
										 nameof(GameFlowParagraphHeader),
										 nameof(GameFlowItem1),
										 nameof(GameFlowItem2),
										 nameof(GameFlowItem3),
										 nameof(GameFlowItem4),
										 nameof(GameFlowItem5),
										 nameof(ExampleParagraphHeader),
										 nameof(ExampleParagraphText),
										 nameof(HintParagrapHeader),
										 nameof(HintParagrapText),
										 nameof(DisplayName));
		}

		protected override void CleanUp()
		{
			CultureManager.CultureChanged -= RefreshCaptions;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}	
}

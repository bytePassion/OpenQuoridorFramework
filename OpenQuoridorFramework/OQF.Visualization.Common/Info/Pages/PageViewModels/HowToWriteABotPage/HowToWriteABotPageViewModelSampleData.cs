using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.HowToWriteABotPage
{
	internal class HowToWriteABotPageViewModelSampleData : IHowToWriteABotPageViewModel
	{
		public string PageHeader => "blubb";
		
		public string GeneralProceedingParagraphHeader => "blubb";
		public string GeneralProceedingItem1           => "blubb";
		public string GeneralProceedingItem2           => "blubb";
		public string GeneralProceedingItem3           => "blubb";
		public string GeneralProceedingItem4           => "blubb";
		public string GeneralProceedingItem5           => "blubb";
		
		public string ImportantClassesParagraphHeader => "blubb";
		public string ImportantClassesParagraphNote   => "blubb";
		public string FieldCoordinateParagraphHeader  => "blubb";
		public string FieldCoordinateParagraphText    => "blubb";
		public string PlayerAndStateParagraphHeader   => "blubb";
		public string PlayerAndStateParagraphText     => "blubb";
		public string WallParagraphHeader             => "blubb";
		public string WallParagraphText               => "blubb";
		public string BoardStateParagraphHeader       => "blubb";
		public string BoardStateParagraphText         => "blubb";
		public string MovesParagraphHeader            => "blubb";
		public string MovesParagraphText              => "blubb";
		public string GameConstraintsParagraphHeader  => "blubb";
		public string GameConstraintsParagraphText    => "blubb";
		
		public string GameFlowParagraphHeader => "blubb";
		public string GameFlowItem1           => "blubb";
		public string GameFlowItem2           => "blubb";
		public string GameFlowItem3           => "blubb";
		public string GameFlowItem4           => "blubb";
		public string GameFlowItem5           => "blubb";

		public string ExampleParagraphHeader => "blubb";
		public string ExampleParagraphText   => "blubb";

		public string HintParagrapHeader => "blubb";
		public string HintParagrapText   => "blubb";

		public string DisplayName => "HowTo";

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;        
    }
}
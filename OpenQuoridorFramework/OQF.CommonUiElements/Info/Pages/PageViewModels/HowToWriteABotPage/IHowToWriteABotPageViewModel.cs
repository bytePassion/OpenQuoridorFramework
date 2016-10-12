using Lib.Wpf.ViewModelBase;

namespace OQF.CommonUiElements.Info.Pages.PageViewModels.HowToWriteABotPage
{
	internal interface IHowToWriteABotPageViewModel : IViewModel, IPage
	{
		string PageHeader { get; }

		string GeneralProceedingParagraphHeader { get; }
		string GeneralProceedingItem1           { get; }
		string GeneralProceedingItem2           { get; }
		string GeneralProceedingItem3           { get; }
		string GeneralProceedingItem4           { get; }
		string GeneralProceedingItem5           { get; }

		string ImportantClassesParagraphHeader { get; }
		string ImportantClassesParagraphNote   { get; }
		string FieldCoordinateParagraphHeader  { get; }
		string FieldCoordinateParagraphText    { get; }
		string PlayerAndStateParagraphHeader   { get; }
		string PlayerAndStateParagraphText     { get; }
		string WallParagraphHeader             { get; }
		string WallParagraphText               { get; }
		string BoardStateParagraphHeader       { get; }
		string BoardStateParagraphText         { get; }
		string MovesParagraphHeader            { get; }
		string MovesParagraphText              { get; }
		string GameConstraintsParagraphHeader  { get; }
		string GameConstraintsParagraphText    { get; }

		string GameFlowParagraphHeader { get; }
		string GameFlowItem1           { get; }
		string GameFlowItem2           { get; }
		string GameFlowItem3           { get; }
		string GameFlowItem4           { get; }
		string GameFlowItem5           { get; }

		string ExampleParagraphHeader { get; }
		string ExampleParagraphText   { get; }

		string HintParagrapHeader { get; }
		string HintParagrapText   { get; }
	}
}
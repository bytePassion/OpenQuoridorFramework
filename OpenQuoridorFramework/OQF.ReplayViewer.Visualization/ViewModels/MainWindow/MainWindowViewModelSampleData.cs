using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using OQF.CommonUiElements.Board.ViewModels.Board;
using OQF.CommonUiElements.Board.ViewModels.BoardLabeling;
using OQF.CommonUiElements.Language.LanguageSelection.ViewModel;
using OQF.ReplayViewer.Visualization.ViewModels.MainWindow.Helper;

#pragma warning disable 0067

namespace OQF.ReplayViewer.Visualization.ViewModels.MainWindow
{
    internal class MainWindowViewModelSampleData : IMainWindowViewModel
    {
        public MainWindowViewModelSampleData()
        {
            BoardViewModel = new BoardViewModelSampleData();
            LanguageSelectionViewModel = new LanguageSelectionViewModelSampleData();
            BoardVerticalLabelingViewModel = new BoardLabelingViewModelSampleData();
            BoardHorizontalLabelingViewModel = new BoardLabelingViewModelSampleData();

            LodingString = "blubb.txt";
            MoveIndex = 5;
            MaxMoveIndex = 15;
            IsReplayLoaded = true;

            ProgressRows = new ObservableCollection<ProgressRow>
            {
                new ProgressRow("1. e2 e8", false),
                new ProgressRow("1. e3 e7", false),
                new ProgressRow("1. e4 e6", false)
            };

            BrowseFileButtonCaption = "Browse";
            InputPromtLabelCaption = "File or compressed Progress";
            LoadAndStartButtonCaption = "Load and start";
            NextStepButtonCaption = "Next";
            PrevStepButtonCaption = "Prev";
            ProgressSectionHeader = "Progress";
            ShowAboutHelpButtonCaption = "Help & About";
        }

        public IBoardViewModel BoardViewModel { get; }
        public ILanguageSelectionViewModel LanguageSelectionViewModel { get; }
        public IBoardLabelingViewModel BoardHorizontalLabelingViewModel { get; }
        public IBoardLabelingViewModel BoardVerticalLabelingViewModel { get; }

        public ICommand LoadGame      => null;
        public ICommand BrowseFile    => null;
        public ICommand ShowAboutHelp => null;
        public ICommand NextMove      => null;
        public ICommand PreviousMove  => null;

        public int MoveIndex { get; set; }
        public int MaxMoveIndex { get; }
        public bool IsReplayLoaded { get; }
        public ObservableCollection<ProgressRow> ProgressRows { get; }
        public string LodingString { get; set; }


        public string BrowseFileButtonCaption { get; }
        public string InputPromtLabelCaption { get; }
        public string LoadAndStartButtonCaption { get; }
        public string NextStepButtonCaption { get; }
        public string PrevStepButtonCaption { get; }
        public string ProgressSectionHeader { get; }
        public string ShowAboutHelpButtonCaption { get; }

        public void Dispose () { }
        public event PropertyChangedEventHandler PropertyChanged;		
    }
}
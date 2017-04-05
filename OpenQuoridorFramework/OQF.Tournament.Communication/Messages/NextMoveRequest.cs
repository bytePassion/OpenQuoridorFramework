using OQF.AnalysisAndProgress.ProgressUtils;

namespace OQF.Tournament.Communication.Messages
{
    public class NextMoveRequest : NetworkMessageBase
    {
        public QProgress CurrentProgress { get; }

        public NextMoveRequest(QProgress progress) : base(NetworkMessageType.NextMoveRequest)
        {
            CurrentProgress = progress;
        }

        public override string AsString()
        {
        return CurrentProgress.Compressed;
        }



        public static NextMoveRequest Parse(string msgContent)
        {
            var progress = CreateQProgress.FromCompressedProgressString(msgContent);
            return new NextMoveRequest(progress);
        }
    }
}
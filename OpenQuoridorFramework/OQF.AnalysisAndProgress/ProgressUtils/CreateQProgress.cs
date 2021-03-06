﻿using System.Collections.Generic;
using System.Linq;
using OQF.AnalysisAndProgress.ProgressUtils.Coding;
using OQF.AnalysisAndProgress.ProgressUtils.Parsing;
using OQF.Bot.Contracts.GameElements;
using OQF.Bot.Contracts.Moves;
using OQF.Utils.BoardStateUtils;

namespace OQF.AnalysisAndProgress.ProgressUtils
{
    public static class CreateQProgress
    {
        public static QProgress FromMoveList(IEnumerable<Move> moveList)
        {
            return new QProgress(moveList);
        }

        public static QProgress FromCompressedProgressString(string compressedProgressString)
        {
            return new QProgress(ProgressCoding.CompressedStringToMoveList(compressedProgressString));
        }

        public static QProgress FromReadableProgressTextFile(string progressText)
        {
            return new QProgress(ParseProgressText.FromFileText(progressText)
                                                  .Select(MoveParser.GetMove));
        }

        public static QProgress FromBoardState(BoardState boardState)
        {
            return new QProgress(boardState.GetMoveList());			
        }
    }
}
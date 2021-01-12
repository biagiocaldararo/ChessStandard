using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class Bishop : LongRangePiece
    {
        public Bishop(int id, Faction faction, Square square) : base(id, faction, square)
        {
            BoardElements = new List<BoardElement>() {
                BoardElement.MainDiagonal,
                BoardElement.AntiDiagonal,
            };
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            return GetLegalSquares(board, this, BoardElements);
        }

    }
}
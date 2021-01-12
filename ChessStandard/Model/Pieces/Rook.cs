using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class Rook : LongRangePiece
    {
        public Rook(int id, Faction faction, Square square, BoardSide side) : base(id, faction, square)
        {
            BoardElements = new List<BoardElement>() {
                BoardElement.Row,
                BoardElement.Column,
            };
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            return GetLegalSquares(board, this, BoardElements);
        }
    }
}

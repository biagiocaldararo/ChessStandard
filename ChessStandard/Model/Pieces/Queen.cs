using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class Queen : LongRangePiece
    {
        private readonly static List<BoardElement> _BoardElements = GetEnums<BoardElement>();

        public Queen(int id, Faction faction, Square square) : base(id, faction, square)
        {
            BoardElements = _BoardElements;
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            return GetLegalSquares(board, this, BoardElements);
        }

        public static List<Square> GetLegalSquares(ChessBoard board, Piece piece)
        {
            return GetLegalSquares(board, piece, _BoardElements);
        }
    }
}
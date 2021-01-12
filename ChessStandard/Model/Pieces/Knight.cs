using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class Knight : Piece
    {
        public readonly List<Square> SQUARE_RANGE = new List<Square>()
        {
            new Square( 2, 1),
            new Square( 2,-1),
            new Square( 1, 2),
            new Square( 1,-2),
            new Square(-2, 1),
            new Square(-2,-1),
            new Square(-1, 2),
            new Square(-1,-2),
        };

        public Knight(int id, Faction faction, Square square) : base(id, faction, square)
        {
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            var legalSquares = new List<Square>();

            foreach (var s in SQUARE_RANGE)
            {
                var square = new Square(Square.Row + s.Row, Square.Column + s.Column);

                if (square.Legal)
                {
                    var pieceOnSquare = board.GetPieceBySquare(square);

                    if (pieceOnSquare == null || (pieceOnSquare != null && pieceOnSquare.Faction != Faction))
                    {
                        legalSquares.Add(square);
                    }
                }
            }

            return legalSquares;
        }
    }
}
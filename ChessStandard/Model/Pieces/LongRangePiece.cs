using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public abstract class LongRangePiece : Piece
    {
        protected List<BoardElement> BoardElements { get; set; }

        protected LongRangePiece(int id, Faction faction, Square square) : base(id, faction, square)
        {
            
        }

        private static List<Square> GetLegalSquares(ChessBoard board, Piece piece, BoardElement element)
        {
            var legalSquares = new List<Square>();
            var directions = Direction.Default();

            bool done = false;

            for (int i = 1; i < ChessBoard.MAX_ROW_COLUMN && !done; i++)
            {
                foreach (var dir in directions)
                {
                    if (!dir.Closed)
                    {
                        int n = dir.Value * i;
                        var square = piece.Square.GetNext(n, element);

                        if (square.Legal)
                        {
                            var pieceOnSquare = board.GetPieceBySquare(square);

                            if (pieceOnSquare == null || (pieceOnSquare != null && pieceOnSquare.Faction != piece.Faction))
                            {
                                legalSquares.Add(square);
                                dir.Closed = pieceOnSquare != null && pieceOnSquare.Faction != piece.Faction;
                            }
                            else
                            {
                                dir.Closed = true;
                            }
                        }
                        else
                        {
                            dir.Closed = true;
                        }
                    }
                }

                done = Direction.AllClosed(directions);
            }

            return legalSquares;
        }

        protected static List<Square> GetLegalSquares(ChessBoard board, Piece piece, List<BoardElement> boardElements)
        {
            var legalSquares = new List<Square>();

            boardElements.ForEach(dir => legalSquares.AddRange(GetLegalSquares(board, piece, dir)));

            return legalSquares;
        }
    }
}


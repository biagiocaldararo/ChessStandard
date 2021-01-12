using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class King : Piece
    {
        public readonly List<Square> SQUARE_RANGE = new List<Square>()
        {
            new Square( 1, 0),
            new Square( 1, 1),
            new Square( 1,-1),
            new Square( 0, 1),
            new Square( 0,-1),
            new Square(-1, 0),
            new Square(-1, 1),
            new Square(-1,-1),
        };
        public readonly List<Square> SQUARES_WITH_ROOK = new List<Square>()
        {
            new Square( 1, 1),
            new Square( 1, 8),
            new Square( 8, 1),
            new Square( 8, 8),
        };

        public bool Castled { get; set; }

        public King(int id, Faction faction, Square square) : base(id, faction, square)
        {
            Castled = false;
        }

        public override void SetSquare(Square square, ChessBoard board)
        {
            base.SetSquare(square, board);

            if (square.Castling)
            {
                var rook = GetRook(board, Side, false);

                if (rook != null)
                {
                    var newRookSquare = new Square(Square.Row, Square.Column + (int)rook.Side);
                    rook.SetSquare(newRookSquare, board);
                    Castled = true;
                }
            }
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            var legalSquares = new List<Square>();

            legalSquares.AddRange(Movement(board, threat));

            if (!Castled && !threat)
            {
                legalSquares.AddRange(Castling(board));
            }

            return legalSquares;
        }

        private List<Square> Movement(ChessBoard board, bool threat)
        {
            var legalSquares = new List<Square>();

            foreach (var s in SQUARE_RANGE)
            {
                var square = new Square(Square.Row + s.Row, Square.Column + s.Column);

                if (square.Legal)
                {
                    if (threat || !square.Threatened(Faction, board))
                    {
                        var pieceOnSquare = board.GetPieceBySquare(square);

                        if (pieceOnSquare == null || (pieceOnSquare != null && pieceOnSquare.Faction != Faction))
                        {
                            legalSquares.Add(square);
                        }
                    }
                }
            }

            return legalSquares;
        }

        private List<Square> Castling(ChessBoard board)
        {
            var legalSquares = new List<Square>();

            foreach (var type in GetEnums<BoardSide>())
            {
                var castling = Castling(board, type);

                if (castling != null)
                {
                    legalSquares.Add(castling);
                }
            }

            return legalSquares;
        }

        private Square Castling(ChessBoard board, BoardSide side)
        {
            Square square = null;

            if (!Moved && !IsChecked(board))
            {
                var rook = GetRook(board, side, true);

                if (rook != null && !rook.Moved)
                {
                    int direction = -(int)rook.Side;

                    if (CheckSquaresBetween(board, rook, direction))
                    {
                        square = new Square(Square.Row, Square.Column + (direction * 2));
                        square.Castling = true;
                    }
                }
            }

            return square;
        }

        private Rook GetRook(ChessBoard board, BoardSide side, bool castling)
        {
            Rook rook = null;
            var boardSide = side;

            foreach (var square in SQUARES_WITH_ROOK)
            {
                if (!castling)
                {
                    boardSide = square.BoardSide;
                }

                var piece = board.GetPieceBySquare(square);

                if (piece != null && piece.GetType() == typeof(Rook) &&
                    piece.Faction == Faction && piece.Side == boardSide)
                {
                    rook = (Rook)piece;
                }
            }

            return rook;
        }

        private bool CheckSquaresBetween(ChessBoard board, Rook rook, int direction)
        {
            bool isChecked = true;

            bool done = false;
            int i = 1;

            do
            {
                var square = Square.GetNext(direction * i, BoardElement.Column);

                if (square.Column != rook.Square.Column)
                {
                    isChecked = board.GetPieceBySquare(square) == null && !square.Threatened(Faction, board);
                    i++;
                }
                else
                {
                    done = true;
                }

            } while (isChecked && !done);

            return isChecked;
        }

        private bool IsChecked(ChessBoard board)
        {
            return Square.Threatened(Faction, board);
        }
    }
}
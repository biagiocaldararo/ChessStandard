using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public class Pawn : Piece
    {
        public readonly List<Direction> DIRECTIONS = Direction.Default();
        public readonly int MOVEMENT_RANGE = 2;

        public bool Promoted { get; set; }

        public Pawn(int id, Faction faction, Square square) : base(id, faction, square)
        {
            Promoted = false;
        }

        public override void SetSquare(Square square, ChessBoard board)
        {
            Promoted = (Faction == Faction.White && square.Row == ChessBoard.MAX_ROW_COLUMN) ||
                (Faction == Faction.Black && square.Row == ChessBoard.MIN_ROW_COLUMN);

            base.SetSquare(square, board);
        }

        public override List<Square> GetLegalSquares(ChessBoard board, bool threat)
        {
            var legalSquares = new List<Square>();

            if (!Promoted)
            {
                if (!threat)
                {
                    legalSquares.AddRange(MoveForward(board));
                }

                legalSquares.AddRange(Capture(board));

                if (!threat)
                {
                    var enPassantSquare = EnPassantCapture(board);

                    if (enPassantSquare != null)
                    {
                        legalSquares.Add(enPassantSquare);
                    }
                }
            }
            else
            {
                legalSquares = Queen.GetLegalSquares(board, this);
            }

            return legalSquares;
        }

        private List<Square> MoveForward(ChessBoard board)
        {
            int i = (int)Faction;
            List<Square> legalSquares = new List<Square>();

            for (int j = 1; j <= MOVEMENT_RANGE; j++)
            {
                if (!Moved || j == 1)
                {
                    var square = new Square(Square.Row + (j * i), Square.Column);

                    if (square.Legal && board.GetPieceBySquare(square) == null)
                    {
                        legalSquares.Add(square);
                    }
                }
            }

            return legalSquares;
        }

        private List<Square> Capture(ChessBoard board)
        {
            int i = (int)Faction;
            var legalSquares = new List<Square>();

            foreach (var d in DIRECTIONS)
            {
                var legalSquare = new Square(Square.Row + i, Square.Column + d.Value);
                var piece = board.GetPieceBySquare(legalSquare);

                if (piece != null && piece.Faction != Faction)
                {
                    legalSquares.Add(legalSquare);
                }
            }

            return legalSquares;

        }

        private Square EnPassantCapture(ChessBoard board)
        {
            int i = (int)Faction;
            Square square = null;

            foreach (var d in DIRECTIONS)
            {
                var legalSquare = new Square(Square.Row, Square.Column + d.Value);
                var piece = board.GetPieceBySquare(legalSquare);

                if (piece != null && piece.Faction != Faction && EnPassantAvailable(board, piece))
                {
                    legalSquare.Row = this.Square.Row + i;
                    legalSquare.EnPassant = true;

                    square = legalSquare;

                    break;
                }
            }

            return square;
        }

        private bool EnPassantAvailable(ChessBoard board, Piece piece)
        {
            bool available = false;

            var lastMove = board.History.GetLastMove();

            if (lastMove != null && lastMove.Piece.GetType() == typeof(Pawn))
            {
                bool pawnMadeAMoveOfTwo = Math.Abs(lastMove.SquareTo.Row - lastMove.SquareFrom.Row) == 2;
                available = pawnMadeAMoveOfTwo && lastMove.Piece.Id == piece.Id;
            }

            return available;
        }
    }
}

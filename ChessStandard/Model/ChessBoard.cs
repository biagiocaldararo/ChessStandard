using ChessStandard.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public sealed class ChessBoard
    {
        public static readonly int MIN_ROW_COLUMN = 1;
        public static readonly int MAX_ROW_COLUMN = 8;

        #region Singleton
        private static readonly Lazy<ChessBoard> _Instance = new Lazy<ChessBoard>(() => new ChessBoard());

        public static ChessBoard Instance
        {
            get
            {
                return _Instance.Value;
            }
        }
        #endregion

        public List<Piece> Pieces { get; set; }
        public History History { get; private set; }

        private ChessBoard()
        {
            Init();
        }

        private void Init()
        {
            Pieces = new List<Piece>()
            {
                new Rook(1, Faction.White, new Square(1, 1), BoardSide.Queen),
                new Knight(2, Faction.White, new Square(1, 2)),
                new Bishop(3, Faction.White, new Square(1, 3)),
                new Queen(4, Faction.White, new Square(1, 4)),
                new King(5, Faction.White, new Square(1, 5)),
                new Bishop(6, Faction.White, new Square(1, 6)),
                new Knight(7, Faction.White, new Square(1, 7)),
                new Rook(8, Faction.White, new Square(1, 8), BoardSide.King),

                new Pawn(9, Faction.White, new Square(2, 1)),
                new Pawn(10, Faction.White, new Square(2, 2)),
                new Pawn(11, Faction.White, new Square(2, 3)),
                new Pawn(12, Faction.White, new Square(2, 4)),
                new Pawn(13, Faction.White, new Square(2, 5)),
                new Pawn(14, Faction.White, new Square(2, 6)),
                new Pawn(15, Faction.White, new Square(2, 7)),
                new Pawn(16, Faction.White, new Square(2, 8)),

                new Rook(17, Faction.Black, new Square(8, 1), BoardSide.Queen),
                new Knight(18, Faction.Black, new Square(8, 2)),
                new Bishop(19, Faction.Black, new Square(8, 3)),
                new Queen(20, Faction.Black, new Square(8, 4)),
                new King(21, Faction.Black, new Square(8, 5)),
                new Bishop(22, Faction.Black, new Square(8, 6)),
                new Knight(23, Faction.Black, new Square(8, 7)),
                new Rook(24, Faction.Black, new Square(8, 8), BoardSide.King),

                new Pawn(25, Faction.Black, new Square(7, 1)),
                new Pawn(26, Faction.Black, new Square(7, 2)),
                new Pawn(27, Faction.Black, new Square(7, 3)),
                new Pawn(28, Faction.Black, new Square(7, 4)),
                new Pawn(29, Faction.Black, new Square(7, 5)),
                new Pawn(30, Faction.Black, new Square(7, 6)),
                new Pawn(31, Faction.Black, new Square(7, 7)),
                new Pawn(32, Faction.Black, new Square(7, 8))
            };

            History = History.Instance;
        }

        public Move Move(Faction playerFaction, Piece piece, Square newSquare)
        {
            Move move = null;

            if (playerFaction == piece.Faction && CheckLegalSquare(piece, newSquare))
            {
                move = new Move(this, piece, newSquare, playerFaction);
                Update(move, piece);
            }

            return move;
        }

        public bool CheckLegalSquare(Piece piece, Square square)
        {
            bool legal = false;
            var legalSquares = piece.GetLegalSquares(this, false);

            foreach (var legalSquare in legalSquares)
            {
                if (legalSquare.Compare(square))
                {
                    square.EnPassant = legalSquare.EnPassant;
                    square.Castling = legalSquare.Castling;
                    legal = true;
                    break;
                }
            }

            return legal;
        }

        public void Update(Move move, Piece piece)
        {
            if (move.Capture)
            {
                RemovePiece(move.CapturedPiece);
            }

            piece.SetSquare(move.SquareTo, this);
            move.IsCheck(this);
        }

        public void RemovePiece(Piece piece)
        {
            bool removed = false;

            for (int i = 0; i < Pieces.Count && !removed; i++)
            {
                if (Pieces[i].Id == piece.Id)
                {
                    Pieces[i].Captured = true;
                    removed = true;
                }
            }
        }

        public Piece GetPieceBySquare(Square square)
        {
            Piece piece = null;

            bool founded = false;

            for (int i = 0; i < Pieces.Count && !founded; i++)
            {
                if (!Pieces[i].Captured && Pieces[i].Square.Compare(square))
                {
                    piece = Pieces[i];
                    founded = true;
                }
            }

            return piece;
        }

        public static Square GetSquare(string desc)
        {
            Square square = null;

            if (desc.Length == 2 &&
                Enum.TryParse(desc.Substring(0, 1), out Column col) &&
                Int32.TryParse(desc.Substring(1, 1), out int row))
            {
                square = new Square(row, (int)col);
            }

            return square;
        }
    }
}

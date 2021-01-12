using ChessStandard.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public class Move
    {
        public Piece Piece { get; }
        public Square SquareFrom { get; }
        public Square SquareTo { get; }
        public Faction Faction { get; }
        public Piece CapturedPiece { get; }
        public bool Capture
        {
            get
            {
                return CapturedPiece != null;
            }
        }
        public bool Check { get; private set; }
        public bool CheckMate
        {
            get
            {
                return Capture && (CapturedPiece.Faction != Faction && CapturedPiece.GetType() == typeof(King));
            }
        }
        public string Record
        {
            get
            {
                string record = "";

                if (SquareFrom != null && SquareTo != null)
                {
                    if (Piece.GetType() == typeof(King) && SquareTo.Castling)
                    {
                        record = SquareTo.BoardSide == BoardSide.King ? "O-O" : "O-O-O";
                    }
                    else
                    {
                        string isCapture = Capture ? "x" : "";
                        string isCheck = Check ? "+" : "";
                        record = SquareFrom.ToString() + isCapture + SquareTo.ToString() + isCheck;
                    }

                }

                return record;
            }
        }

        public Move(ChessBoard board, Piece piece, Square newSquare, Faction faction)
        {
            Piece = piece;
            SquareFrom = piece.Square;
            SquareTo = newSquare;
            Faction = faction;
            CapturedPiece = SetCapturedPiece(board, newSquare);
            Check = false;
        }

        private Piece SetCapturedPiece(ChessBoard board, Square square)
        {
            var newSquare = square;

            if (square.EnPassant)
            {
                int i = (int)Piece.Faction;
                newSquare = new Square(square.Row - i, square.Column);
            }

            return board.GetPieceBySquare(newSquare);
        }

        public void IsCheck(ChessBoard board)
        {
            Check = Piece.Check(board);
        }
    }
}

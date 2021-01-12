using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model.Pieces
{
    public abstract class Piece
    {
        public int Id { get; }
        public Faction Faction { get; }
        public Square Square { get; private set; }
        public bool Captured { get; set; }
        public bool Moved { get; set; }
        public BoardSide Side { get; }

        protected Piece(int id, Faction faction, Square square)
        {
            Id = id;
            Faction = faction;
            Square = square;
            Captured = false;
            Moved = false;
            Side = SetBoardSide();
        }

        private BoardSide SetBoardSide()
        {
            return Square.Column <= 4 ? BoardSide.Queen : BoardSide.King;
        }

        public virtual void SetSquare(Square square, ChessBoard board)
        {
            Square = square;
            Moved = true;
        }

        public abstract List<Square> GetLegalSquares(ChessBoard board, bool threat);

        public List<string> GetLegalSquaresDescription(ChessBoard board, bool threat)
        {
            var squares = new List<string>();

            GetLegalSquares(board, threat).ForEach(x => squares.Add(x.ToString()));

            return squares;
        }

        public bool Check(ChessBoard board)
        {
            bool check = false;

            var legalSquares = GetLegalSquares(board, true);

            var pieces = board.Pieces;

            for (int i = 0; i < pieces.Count && !check; i++)
            {
                if (!pieces[i].Captured && pieces[i].GetType() == typeof(King) && pieces[i].Faction != Faction)
                {
                    for (int j = 0; j < legalSquares.Count && !check; j++)
                    {
                        if (pieces[i].Square.Compare(legalSquares[j]))
                        {
                            check = true;
                        }
                    }
                }
            }

            return check;
        }
    }

}

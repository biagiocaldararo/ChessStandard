using ChessStandard.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public sealed class History
    {
        #region Singleton
        private static readonly Lazy<History> _Instance = new Lazy<History>(() => new History());

        public static History Instance
        {
            get
            {
                return _Instance.Value;
            }
        }
        #endregion

        private List<Move> Moves;
        public int MoveCount
        {
            get
            {
                return Moves.Count;
            }
        }

        private History()
        {
            Moves = new List<Move>();
        }

        public List<string> Print()
        {
            var history = new List<string>();

            Moves.ForEach(x => history.Add(x.Record));

            return history;
        }

        public void AddMove(Move move)
        {
            Moves.Add(move);
        }

        public Move GetPrevMove(int i)
        {
            Move lastMove = null;

            int count = Moves.Count;

            if (count > 0)
            {
                lastMove = Moves[count - i];
            }

            return lastMove;
        }

        public int MoveCountPiece(Piece piece)
        {
            int movements = 0;

            foreach(var move in Moves)
            {
                if (piece.Id == move.Piece.Id)
                {
                    movements++;
                }
            }

            return movements;
        }

        public void RemoveLastMove()
        {
            Moves.RemoveAt(Moves.Count - 1);
        }
    }
}

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

        public Move GetLastMove()
        {
            Move lastMove = null;
            int count = Moves.Count;

            if (count > 0)
            {
                lastMove = Moves[count - 1];
            }

            return lastMove;
        }
    }
}

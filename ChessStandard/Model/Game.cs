using ChessStandard.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public sealed class Game
    {
        #region Singleton
        private static readonly Lazy<Game> _Instance = new Lazy<Game>(() => new Game());
        public static Game Instance
        {
            get
            {
                return _Instance.Value;
            }
        }
        #endregion

        public ChessBoard Board { get; }
        public List<Player> Players { get; }
        public Player CurrentPlayer { get; private set; }
        public bool HasWinner { get; private set; }

        private Game()
        {
            HasWinner = false;

            Player white = new Player(1, Faction.White);
            Player black = new Player(2, Faction.Black);

            //White moves first
            CurrentPlayer = white;

            Players = new List<Player>(2) { white, black };

            Board = ChessBoard.Instance;
        }

        private void NextPlayerTurn()
        {
            for (int index = 0; index < Players.Count; index++)
            {
                var player = Players[index];

                if (player.Id == CurrentPlayer.Id)
                {
                    //se il giocatore corrente è l'ultimo della lista, il prossimo sarà il primo (in posizione 0)
                    int newIndex = index == Players.Count - 1 ? 0 : (index + 1);
                    CurrentPlayer = Players[newIndex];

                    break;
                }
            }
        }

        public bool MakeAMove(Piece piece, Square newSquare)
        {
            bool result = false;

            if (!HasWinner)
            {
                var move = CurrentPlayer.MoveAPiece(Board, piece, newSquare);

                if (move != null)
                {
                    Board.History.AddMove(move);

                    if (move.CheckMate)
                    {
                        HasWinner = true;
                    }
                    else
                    {
                        NextPlayerTurn();
                    }

                    result = true;
                }
            }

            return result;
        }

        public void UndoMove()
        {
            Board.UndoMove();
            NextPlayerTurn();
        }
    }
}

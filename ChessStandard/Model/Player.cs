using ChessStandard.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public class Player
    {
        public int Id { get; }
        public Faction Faction { get; }

        public Player(int id, Faction faction)
        {
            Id = id;
            Faction = faction;
        }

        public Move MoveAPiece(ChessBoard board, Piece piece, Square newSquare)
        {
            return board.Move(Faction, piece, newSquare);
        }
    }
}

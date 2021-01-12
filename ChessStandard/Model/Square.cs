using ChessStandard.Model.Pieces;
using ChessStandard.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ChessStandard.Utils.Enums;

namespace ChessStandard.Model
{
    public class Square
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool EnPassant { get; set; }
        public bool Castling { get; set; }
        public BoardSide BoardSide
        {
            get
            {
                return Column <= 4 ? BoardSide.Queen : BoardSide.King;
            }
        }
        public bool Legal
        {
            get
            {
                return Row >= ChessBoard.MIN_ROW_COLUMN && Row <= ChessBoard.MAX_ROW_COLUMN &&
                       Column >= ChessBoard.MIN_ROW_COLUMN && Column <= ChessBoard.MAX_ROW_COLUMN;
            }
        }

        public Square(int row, int col)
        {
            Row = row;
            Column = col;
            EnPassant = false;
            Castling = false;
        }

        public bool Compare(Square square)
        {
            return Row == square.Row && Column == square.Column;
        }

        public Square GetNext(int n, BoardElement element)
        {
            var adiacent = new Square(Row, Column);

            switch (element)
            {
                case BoardElement.Row:
                    adiacent.Row += n;
                    break;

                case BoardElement.Column:
                    adiacent.Column += n;
                    break;

                case BoardElement.MainDiagonal:
                    adiacent.Row += n;
                    adiacent.Column += n;
                    break;

                case BoardElement.AntiDiagonal:
                    adiacent.Row += n;
                    adiacent.Column -= n;
                    break;
            }

            return adiacent;
        }

        public bool Threatened(Faction faction, ChessBoard board)
        {
            var pieces = board.Pieces;
            bool threat = false;

            for (int i = 0; i < pieces.Count && !threat; i++)
            {
                if (!pieces[i].Captured && pieces[i].Faction != faction)
                {
                    var squares = pieces[i].GetLegalSquares(board, true);

                    for (int j = 0; j < squares.Count && !threat; j++)
                    {
                        if (Compare(squares[j]))
                        {
                            threat = true;
                        }
                    }
                }
            }

            return threat;
        }

        public override string ToString()
        {
            return (Column)Column + Row.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessStandard.Utils
{
    public class Direction
    {
        public int Value { get; set; }
        public bool Closed { get; set; }

        public Direction(int value)
        {
            Value = value;
            Closed = false;
        }

        public static List<Direction> Default()
        {
            return new List<Direction>()
            {
                new Direction(-1),
                new Direction(1)
            };
        }

        public static bool AllClosed(List<Direction> dir)
        {
            bool close = true;

            dir.ForEach(x => close &= x.Closed);

            return close;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChessStandard.Utils
{
    public class Enums
    {
        public static List<T> GetEnums<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public enum Faction
        {
            White = 1,
            Black = -1
        }

        public enum BoardElement
        {
            Row,
            Column,
            MainDiagonal,
            AntiDiagonal
        }

        public enum Column
        {
            A = 1,
            B = 2,
            C = 3,
            D = 4,
            E = 5,
            F = 6,
            G = 7,
            H = 8
        }

        public enum BoardSide
        {
            Queen = 1,
            King = -1
        }
    }
}

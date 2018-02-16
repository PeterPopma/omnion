using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnion.ChessClient
{
    class ChessMove
    {
        int xFrom;
        int yFrom;
        int xTo;
        int yTo;

        public int XFrom { get => xFrom; set => xFrom = value; }
        public int YFrom { get => yFrom; set => yFrom = value; }
        public int XTo { get => xTo; set => xTo = value; }
        public int YTo { get => yTo; set => yTo = value; }

        public ChessMove(int xFrom, int yFrom, int xTo, int yTo)
        {
            XFrom = xFrom;
            YFrom = yFrom;
            XTo = xTo;
            YTo = yTo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.World
{
    public struct TileData
    {
        public int A;
        public int B;
        public int C;
        public int D;

        public TileData(int a, int b, int c, int d)
        {
            A = a;
            B = b;
            C = c;
            D = c;
        }
    }
}

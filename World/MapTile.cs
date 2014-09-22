using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.World
{
    public class MapTile
    {
        public Terrain.Terrain Terrain;
        //public int X;
        //public int Y;

        public int Depth { get { return Terrain.Depth; } }
        public TileData Data;
    }
}

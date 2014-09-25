using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Square.Mesh
{
    class TileBuffer : PolygonBuffer
    {
        public TileBuffer(Destiny game, int tileCount)
            : base(game, tileCount, Tile.TILE_FACE_COUNT, Tile.TILE_FACE_VERTICES)
        { 
        }

    }
}

using Destiny.Graphics.World.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Square.Mesh
{
    class TileBufferFactory : IPolygonBufferFactory<TileBuffer>
    {
        public TileBuffer CreateBuffer(Destiny game)
        {
            return new TileBuffer(game, 256*256);
        }

    }
}

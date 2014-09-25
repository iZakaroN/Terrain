using Destiny.Graphics.World.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Octagonal.Mesh
{
    class CubeBufferFactory : IPolygonBufferFactory<CubeBuffer>
    {

        public CubeBuffer CreateBuffer(Destiny game)
        {
            return new CubeBuffer(game, 32*32*32);
        }
    }
}

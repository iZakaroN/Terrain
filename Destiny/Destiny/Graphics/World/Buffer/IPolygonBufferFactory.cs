using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Buffer
{
    public interface IPolygonBufferFactory<TB>
                where TB : PolygonBuffer

    {
        TB CreateBuffer(Destiny game);
    }
}

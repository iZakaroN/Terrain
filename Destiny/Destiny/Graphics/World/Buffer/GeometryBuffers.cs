using Destiny.Graphics.World.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World
{
    class GeometryBuffers<TB> : VisualElement
        where TB : PolygonBuffer
    {
        IPolygonBufferFactory<TB> BufferFactory;
        public GeometryBuffers(Destiny game, IPolygonBufferFactory<TB> bufferFactory) : base(game)
        {
            BufferFactory = bufferFactory;
        }

        public TB GetBuffer()
        {
            if (Childs.Count == 0 || ((TB)Childs[0]).IsFull)
            {
                Childs.Insert(0, BufferFactory.CreateBuffer(Game));
            }
            return (TB)Childs[0];
        }
    }
}

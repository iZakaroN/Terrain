using Destiny.Graphics.World.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World
{
	public interface IGeometryBuffers
	{
		PolygonBuffer CurrentBuffer { get; }
		List<VisualElement> Buffers { get; }
	}

	public class GeometryBuffers<TB> : VisualElement, IGeometryBuffers
        where TB : PolygonBuffer
    {
        IPolygonBufferFactory<TB> BufferFactory;
        public GeometryBuffers(Destiny game, IPolygonBufferFactory<TB> bufferFactory) : base(game)
        {
            BufferFactory = bufferFactory;
        }

		public PolygonBuffer CurrentBuffer { get { return GetBuffer(); } }
		public List<VisualElement> Buffers { get { return Childs; } }


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

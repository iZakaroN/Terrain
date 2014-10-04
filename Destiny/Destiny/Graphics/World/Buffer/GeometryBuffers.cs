using Destiny.Graphics.World.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World
{
	public class GeometryBuffers<TB> : VisualElement
		where TB : PolygonBuffer
	{
		IPolygonBufferFactory<TB> BufferFactory;
		TB CurrentBuffer = null;
		public GeometryBuffers(Destiny game, IPolygonBufferFactory<TB> bufferFactory)
			: base(game)
		{
			BufferFactory = bufferFactory;
		}

		protected override void UnloadSelf()
		{
			CurrentBuffer = null;
			RemoveAllChilds();
			base.UnloadSelf();
		}

		/*public TB GetBuffer()
		{
			if (CurrentBuffer == null || CurrentBuffer.IsFull)
			{
				AddChild(CurrentBuffer = BufferFactory.CreateBuffer(Game));
			}
			return CurrentBuffer;
		}*/

		/*public TB GetBuffer()
		{
			if (Childs.Count == 0 || ((TB)Childs[0]).IsFull)
			{
				Childs.Insert(0, BufferFactory.CreateBuffer(Game));
			}
			return (TB)Childs[0];
		}*/
		public void ProcessBuffer(Action<TB> process)
		{
			ProcessBuffer((buffer) => { process(buffer); return true; });
		}

		public T ProcessBuffer<T>(Func<TB,T> process)
		{
			lock (this)
			{
				if (CurrentBuffer == null || CurrentBuffer.IsFull)
				{
					AddChild(CurrentBuffer = BufferFactory.CreateBuffer(Game));
				}
				return process(CurrentBuffer);
			}
		}
	}
}

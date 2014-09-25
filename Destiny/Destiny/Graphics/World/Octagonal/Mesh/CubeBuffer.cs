using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Octagonal.Mesh
{
	public class CubeBuffer : PolygonBuffer
	{
		public CubeBuffer(Destiny game, int cubeCount) : base(game, cubeCount, Cube.CUBE_FACE_COUNT, Cube.CUBE_FACE_VERTICES)
		{
			var a = new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0);
		}

	}
}

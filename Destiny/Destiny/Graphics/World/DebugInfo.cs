using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World
{
	public struct BufferDebugInfo
	{
		public int RegionCount;
		public int FreePolygons;
		public int TotalPolygons;
	}

	public struct TerrainDebugInfo
	{
		public int BuffersCount;
		public BufferDebugInfo CurrentBuffer;

		public static TerrainDebugInfo Empty()
		{
			return new TerrainDebugInfo()
			{
				CurrentBuffer = new BufferDebugInfo(),
			};
		}
	}
}

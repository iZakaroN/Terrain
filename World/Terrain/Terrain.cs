using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.World.Terrain
{
	public class TerrainTypeWeight
	{
		public TerrainType Type;
		public int Weight;
		public TerrainTypeWeight (TerrainType type, int weight)
	{
			Type = type;
			Weight = weight;
	}
	}
	//Define set of terrain type/weight pair
	public class TerrainTile : List<TerrainTypeWeight>
	{
		public TerrainTile() : base()
		{
		}

		public TerrainTile(IEnumerable<TerrainTypeWeight> list) : base(list)
		{
		}
	}

	public class Terrain
	{
		public TerrainTile Type = new TerrainTile() { new TerrainTypeWeight(TerrainType.None, 0 ) };

		public int Depth;

        public int Normal;

	}
}

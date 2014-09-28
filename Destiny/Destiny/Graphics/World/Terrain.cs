using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Destiny.Graphics;
using Destiny.Graphics.World;
using System.Threading;
using Destiny.Graphics.World.Octagonal.Mesh;
using Destiny.Graphics.World.Octagonal.Dimension;
using Destiny.Graphics.World.Square.Mesh;
using Sunshine.World;

namespace Destiny
{
	abstract public class Terrain : VisualElement
    {


		public const int DOWNSCALE = 2;
		public int MapTilesWidth = 1024 / DOWNSCALE;
		public int MapTilesHeight = 1024 / DOWNSCALE;
		public int TerrainDepthScale = 400 * DOWNSCALE;

		VertexDeclaration VertexDeclaration
        {
            get { return VertexPositionNormalTexture.VertexDeclaration; }
        }

		public Terrain(Destiny game)
            : base(game)
        {
        }

		public virtual void AddCube(Vector3 position)
		{
		}

		protected float GetDepth(Sunshine.World.MapTile tile)
		{
			return ((float)tile.Terrain.Depth - Sunshine.World.World.HeightTerrainTypeWater) / TerrainDepthScale;
		}


    }
}

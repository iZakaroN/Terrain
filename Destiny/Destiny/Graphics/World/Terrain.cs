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

        //public const int MAX_CUBECOUNT = 40 * 30 * 30;
        //List<Cube> _cubes = new List<Cube>();

		public const int DOWNSCALE = 1;
		public int MapTilesWidth = 1024 / DOWNSCALE;
		public int MapTilesHeight = 1024 / DOWNSCALE;
		public int TerrainDepthScale = 400/* * DOWNSCALE*/;

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

		/*int _xCount = 50;
		int _yCount = 50;
		int _zCount = 50;

		int CubeCount()
		{
			return _xCount * _yCount * _zCount * 2;
		}

		void CubeIterrator(Action<Vector3> iterrator)
		{
			Vector3 cubeVector = new Vector3(0, 0, 0);
			for (cubeVector.X = 0; cubeVector.X < _xCount; cubeVector.X++)
				for (cubeVector.Y = 0; cubeVector.Y < _yCount; cubeVector.Y++)
					for (cubeVector.Z = 0; cubeVector.Z < _zCount; cubeVector.Z++)
					{
						iterrator(cubeVector * 4);
						iterrator((-Vector3.One - cubeVector));
					}
		}*/



        //Sunshine.World.World HeightFieldTerrain;

    }
}

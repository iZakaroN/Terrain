using Destiny.Graphics.World.Octagonal.Dimension;
using Destiny.Graphics.World.Octagonal.Mesh;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destiny.Graphics.World
{
	class TerrainCube : Terrain<CubeBuffer>
	{
		public const int TILE_GRASS = 0;
		public const int TILE_ROCK = 1;
		public const int TILE_DIRTH = 2;
		public const int TILE_DIRTH_GRASS = 3;
		public const int TILE_WOOD = 4;
		public const int TILE_STONE_WALL2 = 5;
		public const int TILE_STONE_WALL = 6;
		public const int TILE_BRICK_WALL = 7;
		public const int TILE_ROCK_WALL = 8;
		public const int TILE_BLACK = 9;
		public const int TILE_SAND = 10;
		public const int TILE_TEXT = 11;
		public const int TILE_LAVA = 12;

		Dimension _dimension = new Dimension();
		Texture2D _texture;

		List<TextureTile> _terrainTextures = new List<TextureTile>();
		public List<TextureTile> _cube_grass;
		public List<TextureTile> _cube_text;
		public List<TextureTile> _cube_lava;
		public List<TextureTile> _cube_dirth;
		public List<TextureTile> _cube_rock;

		public TerrainCube(Destiny game)
			: base(game, new GeometryBuffers<CubeBuffer>(game, new CubeBufferFactory()))
        {
		}

		List<TextureTile> GetTileSet(params int[] tiles)
		{
			return tiles.ToList().ConvertAll((tileNum) => _terrainTextures[tileNum]);
		}

		public override void AddCube(Vector3 position)
		{
			AddCube(_cube_rock, position);
		}

		public void AddCube(List<TextureTile> textureSet, Vector3 position, bool addToBuffer = true)
		{
			var cube = new Cube(textureSet);
			var oVector = _dimension.AllocateNode(position, cube);
			//if (addToBuffer)
				cube.AddToBuffer(Buffers.GetBuffer(), _dimension.ToWorldVector(oVector));
		}

		void SetZero()
		{
			/*			_dimension._rootVector = new OctagonalVector(0, 0, 0);
						_dimension._root = new Octant(0, new Octant(1, new Cube(_cube_lava)));
						_dimension._depth = 2;*/
			//			_buffers = new List<CubeBuffer>();

			//			AddCube(_cube_text, new Vector3(-1, -1, -1));

			AddCube(_cube_lava, new Vector3(1, 0, 0));
			AddCube(_cube_lava, new Vector3(0, 1, 0));
			AddCube(_cube_lava, new Vector3(0, 0, 1));
			AddCube(_cube_lava, new Vector3(-1, 0, 0));
			AddCube(_cube_lava, new Vector3(0, -1, 0));
			AddCube(_cube_lava, new Vector3(0, 0, -1));

			/*			AddCube(_cube_lava, new Vector3(1, 0, 0));
						AddCube(_cube_lava, new Vector3(-1, 0, 0));
						AddCube(_cube_lava, new Vector3(2, 0, 0));
						AddCube(_cube_lava, new Vector3(-2, 0, 0));*/

			/*			AddCube(_cube_grass, new Vector3(-1, 0, 0));
						AddCube(_cube_grass, new Vector3(-1, -1, 0));
						AddCube(_cube_grass, new Vector3(-1, -2, 0));*/

			/*			CubeIterrator((vector) =>
							{
								AddCube(_cube_lava, vector);
								});*/
			//			_dimension.GetIterrator(SetupDimension).Iterrate();

		}

		private void SetupCubeHeightFieldTerrain()
		{
			var HeightFieldTerrain = new Sunshine.World.World(MapTilesWidth, MapTilesHeight);
			for (int y = 0; y < MapTilesHeight; y++)
				for (int x = 0; x < MapTilesHeight; x++)
				{
					var tile = HeightFieldTerrain.GetMapTile(x, y);
					var tileDepth = GetDepth(tile);
					int l = Math.Max(0, x - 1);
					int r = Math.Min(MapTilesWidth, x + 1);
					int t = Math.Max(0, y - 1);
					int b = Math.Min(MapTilesHeight, y + 1);
					float minDepth = Min(
						GetDepth(HeightFieldTerrain.GetMapTile(l, y)),
						GetDepth(HeightFieldTerrain.GetMapTile(r, y)),
						GetDepth(HeightFieldTerrain.GetMapTile(x, t)),
						GetDepth(HeightFieldTerrain.GetMapTile(x, b))
						);
					for (int d = (int)minDepth + 1; d < tileDepth; d++)
						AddCube(_cube_dirth, new Vector3(x - MapTilesWidth / 2, d, y - MapTilesHeight / 2), false);

					AddCube(_cube_grass, new Vector3(x - MapTilesWidth / 2, tileDepth, y - MapTilesHeight / 2), false);

				}
		}

		float Min(params float[] values)
		{
			float result = values[0];
			for (int i = 1; i < values.Count(); i++)
				result = Math.Min(result, values[i]);
			return result;
		}

		override public void LoadContent()
		{
			_texture = Content.Load<Texture2D>(@"Textures\terrain");

			_terrainTextures.Add(new TextureTile(_texture, 0, 0));//0
			_terrainTextures.Add(new TextureTile(_texture, 1, 0));//1
			_terrainTextures.Add(new TextureTile(_texture, 2, 0));//2
			_terrainTextures.Add(new TextureTile(_texture, 3, 0));//3
			_terrainTextures.Add(new TextureTile(_texture, 4, 0));//4
			_terrainTextures.Add(new TextureTile(_texture, 5, 0));//5
			_terrainTextures.Add(new TextureTile(_texture, 6, 0));//6
			_terrainTextures.Add(new TextureTile(_texture, 7, 0));//7
			_terrainTextures.Add(new TextureTile(_texture, 0, 1));//8
			_terrainTextures.Add(new TextureTile(_texture, 1, 1));//9
			_terrainTextures.Add(new TextureTile(_texture, 2, 1));//10
			_terrainTextures.Add(new TextureTile(_texture, 15, 4));//11
			_terrainTextures.Add(new TextureTile(_texture, 15, 15));//12

			_cube_grass = GetTileSet(TILE_GRASS, TILE_DIRTH, TILE_DIRTH_GRASS, TILE_DIRTH_GRASS, TILE_DIRTH_GRASS, TILE_DIRTH_GRASS);
			_cube_text = GetTileSet(TILE_ROCK, TILE_TEXT, TILE_TEXT, TILE_TEXT, TILE_TEXT, TILE_TEXT);
			_cube_lava = GetTileSet(TILE_LAVA, TILE_LAVA, TILE_LAVA, TILE_LAVA, TILE_LAVA, TILE_LAVA);
			_cube_dirth = GetTileSet(TILE_DIRTH, TILE_DIRTH, TILE_DIRTH, TILE_DIRTH, TILE_DIRTH, TILE_DIRTH);
			_cube_rock = GetTileSet(TILE_ROCK, TILE_ROCK, TILE_ROCK, TILE_ROCK, TILE_ROCK, TILE_ROCK);
			base.LoadContent();

		}

		public override void SetpVertices()
		{
			SetZero();
			SetupCubeHeightFieldTerrain();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			Effect.Parameters["xTexture"].SetValue(_texture);
			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Buffers.Draw(gameTime);
			}

			/*Effect.CurrentTechnique = Effect.Techniques["ColoredNoShading"];
			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_dimension.GetIterrator(DrawOctant).Iterrate();
			}*/


		}

		void DrawOctant(Octant octant, List<int> path)
		{
			Device.DrawUserPrimitives(PrimitiveType.LineList, _dimension.GetPathLines(path, octant.Node == null ? Color.Yellow : Color.White), 0, 12);
		}

		/*private void SetupDimension(Octant node, List<int> path)
        {
            if (node.Node != null)
                node.Node.AddToBuffer(GetCubeBuffer(), _dimension.ToWorldVector(OctagonalVector.CalculateVector(path)));

        }*/



	}
}

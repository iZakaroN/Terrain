using Destiny.Graphics.World.Square.Mesh;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sunshine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World
{
	class TerrainTile : Terrain
	{
		public const int TILE_SNOW = 0;
		public const int TILE_ROCK = 1;
		public const int TILE_GRASS = 2;
		public const int TILE_SAND = 3;

		Texture2D _textureSnow;
		Texture2D _textureRock;
		Texture2D _textureGras;
		Texture2D _textureSand;

		List<TextureTile> _terrainTextures = new List<TextureTile>();

		GeometryBuffers<TileBuffer> Buffers;

		public TerrainTile(Destiny game)
			: base(game)
		{
			Buffers = new GeometryBuffers<TileBuffer>(game, new TileBufferFactory());
			this.Game.World.DebugUI.AddText("Number of buffers {0}", GetBuffersCount);
		}

		private object GetBuffersCount()
		{
			return Buffers.Childs.Count;
		}

		private void AddTile(TextureTile texture, MapTile[] tiles, Vector3 vector3)
		{
			var tile = new Tile(texture, tiles, TerrainDepthScale);
			tile.AddToBuffer(Buffers.GetBuffer(), vector3);
		}

		private void SetupTileHeightFieldTerrain()
		{
			var HeightFieldTerrain = new Sunshine.World.World(MapTilesWidth, MapTilesHeight);
			for (int y = 0; y < MapTilesHeight; y++)
				for (int x = 0; x < MapTilesHeight; x++)
				{
					MapTile[] tileHights = new MapTile[4] 
                    {
                        HeightFieldTerrain.GetMapTile(x, y),
                        HeightFieldTerrain.GetMapTile(x+1, y),
                        HeightFieldTerrain.GetMapTile(x+1, y+1),
                        HeightFieldTerrain.GetMapTile(x, y+1),
                    };
					//var tileDepth = GetDepth(tile);
					int l = Math.Max(0, x - 1);
					int r = Math.Min(MapTilesWidth, x + 1);
					int t = Math.Max(0, y - 1);
					int b = Math.Min(MapTilesHeight, y + 1);

					AddTile(_terrainTextures[TILE_ROCK], tileHights, new Vector3(x - MapTilesWidth / 2, 0, y - MapTilesHeight / 2));

				}
			GC.Collect(GC.MaxGeneration);
		}

		override public void LoadContent()
		{
			base.LoadContent();
			_textureSnow = Content.Load<Texture2D>(@"Textures\snow");
			_textureRock = Content.Load<Texture2D>(@"Textures\rock");
			_textureGras = Content.Load<Texture2D>(@"Textures\grass");
			_textureSand = Content.Load<Texture2D>(@"Textures\sand");

			_terrainTextures.Add(new TextureTile(_textureSnow, _textureSnow.Bounds.Width / 16, 0, 0));
			_terrainTextures.Add(new TextureTile(_textureRock, _textureRock.Bounds.Width / 16, 0, 0));
			_terrainTextures.Add(new TextureTile(_textureGras, _textureGras.Bounds.Width / 16, 0, 0));
			_terrainTextures.Add(new TextureTile(_textureSand, _textureSand.Bounds.Width / 16, 0, 0));

			SetupTileHeightFieldTerrain();
			Buffers.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			Effect.Parameters["xTexture"].SetValue(_textureRock);
			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Buffers.Draw(gameTime);
			}
		}

	}
}

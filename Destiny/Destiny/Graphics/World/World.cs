using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Destiny.Graphics.UI;

namespace Destiny.Graphics.World
{
	public class World : VisualElement, IDisposable
	{
		public Terrain Terrain;
		TerrainCube TerrainCube;
		TerrainTile TerrainTile;

		public MainUI UI;
		public Avatar Avatar;

		bool cubicTerrain = false;

		public World(Destiny game) : base(game)
		{

			UI = new MainUI(game);
			Childs.Add(UI);

			Avatar = new Avatar(game);
			Childs.Add(Avatar);

		}

		public override void LoadContent()
		{
			base.LoadContent();
			this.Game.World.UI.DebugUI.Debug("Number of buffers: {0}", () => Terrain.GetBuffers().Buffers.Count);
			this.Game.World.UI.DebugUI.Debug("Current Buffer Regions: {0}", () => Terrain.GetBuffers().CurrentBuffer.RegionCount);
			this.Game.World.UI.DebugUI.Debug("Current Buffer Free Polygons: {0}", () => Terrain.GetBuffers().CurrentBuffer.FreePolygons);
			this.Game.World.UI.DebugUI.Debug("Mesh: {0}", () => Terrain.GetBuffers().CurrentBuffer.FreePolygons);
			SetActiveTerrain();
		}

		private void SetActiveTerrain()
		{
			if (cubicTerrain)
				Terrain = GetTerrainCube();
			else
				Terrain = GetTerrainTile();
		}

		private Terrain GetTerrainCube()
		{
			if (TerrainCube == null)
			{
				TerrainCube = new TerrainCube(Game);
				TerrainCube.LoadContent();
			}
			return TerrainCube;
		}

		private Terrain GetTerrainTile()
		{
			if (TerrainTile == null)
			{
				TerrainTile = new TerrainTile(Game);
				TerrainTile.LoadContent();
			}
			return TerrainTile;
		}

		public void SwitchTerrain()
		{
			cubicTerrain = !cubicTerrain;
			SetActiveTerrain();
		}
		
		public override void Draw(GameTime gameTime)
		{
			Terrain.Draw(gameTime);
			base.Draw(gameTime);
		}
    }
}

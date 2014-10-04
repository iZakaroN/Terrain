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
		public ITerrain Terrain;
		ITerrain _terrainCube;
		ITerrain _terrainTile;

		public MainUI UI;
		public Avatar Avatar;

		bool cubicTerrain = true;

		public World(Destiny game) : base(game)
		{

			UI = new MainUI(game);
			AddChild(UI);

			Avatar = new Avatar(game);
			AddChild(Avatar);

		}

		TerrainDebugInfo DebugInfo = TerrainDebugInfo.Empty();


		protected override void LoadSelf()
		{
			this.Game.World.UI.DebugUI.Debug("Number of buffers: {0}", () => DebugInfo.BuffersCount);
			this.Game.World.UI.DebugUI.Debug("Current Buffer Regions: {0}", () => DebugInfo.CurrentBuffer.RegionCount);
			this.Game.World.UI.DebugUI.Debug("Current Buffer Free Polygons: {0}", () => DebugInfo.CurrentBuffer.FreePolygons);
			this.Game.World.UI.DebugUI.Debug("Current Buffer Total Polygons: {0}", () => DebugInfo.CurrentBuffer.TotalPolygons);
			this.Game.World.UI.DebugUI.Debug("Position: {0}:{1}:{2}", () => Avatar.Position.X, () => Avatar.Position.Y, () => Avatar.Position.Z);
			SetActiveTerrain();
		}


		private void SetActiveTerrain()
		{
			if (Terrain != null)
			{
				Terrain.Visible = Terrain.Active = false;
			}
			Terrain = cubicTerrain ? TerrainCube : TerrainTile;
			Terrain.Visible = Terrain.Active = true;
		}

		ITerrain TerrainCube
		{
			get
			{
				if (_terrainCube != null)
				{
					RemoveChild(_terrainCube);
					_terrainCube = null;
				}

				if (_terrainCube == null)
				{
					_terrainCube = new TerrainCube(Game) { Visible = false };
					AddChild(_terrainCube);
				}
				return _terrainCube;
			}
		}

		private ITerrain TerrainTile
		{
			get
			{
				if (_terrainTile != null)
				{
					RemoveChild(_terrainTile);
					_terrainTile = null;
				}

				if (_terrainTile == null)
				{
					_terrainTile = new TerrainTile(Game) { Visible = false };
					AddChild(_terrainTile);

				}
				return _terrainTile;
			}
		}

		public void SwitchTerrain()
		{
			cubicTerrain = !cubicTerrain;
			SetActiveTerrain();
		}

		#region VisualElement overrides
		protected override void UpdateSelf(GameTime gameTime)
		{
			if (Terrain != null)
			{
				DebugInfo = Terrain.GetDebugInfo();
			}
		}
		#endregion VisualElement overrides
	}
}

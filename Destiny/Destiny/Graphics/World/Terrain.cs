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
using System.Threading.Tasks;
using Destiny.Graphics.World.Buffer;

namespace Destiny
{

	public interface ITerrain : IVisualElement
	{
		void SetMapPoint(Vector3 position);
		void SetPointing(Vector3 position);
		void SwitchPointing();
		TerrainDebugInfo GetDebugInfo();
	}

	abstract class Terrain : VisualElement, ITerrain
	{

		bool Pointing;
		Vector3 LastPointingLocation;
		Vector3 NewPointingLocation;


		public Terrain(Destiny game)
			: base(game)
		{
		}

		public virtual void SetMapPoint(Vector3 position)
		{
		}

		public void SwitchPointing()
		{
			Pointing = !Pointing;
		}

		public void SetPointing(Vector3 position)
		{
			NewPointingLocation = position;
		}

		protected override void UpdateSelf(GameTime gameTime)
		{
			base.UpdateSelf(gameTime);
			/*if (LastPointingLocation != NewPointingLocation)
			{
				HidePointingLocation();
				if (Pointing)
				{
					ShowPointingLocation(NewPointingLocation);
					LastPointingLocation = NewPointingLocation;

				}
			}*/
		}

		protected virtual void HidePointingLocation()
		{
		}

		protected virtual void ShowPointingLocation(Vector3 NewPointingLocation)
		{
		}

		public abstract TerrainDebugInfo GetDebugInfo();
	}

	abstract class Terrain<B> : Terrain
		where B : PolygonBuffer
	{


		public const int DOWNSCALE = 2;
		public int MapTilesWidth = 1024 / DOWNSCALE;
		public int MapTilesHeight = 1024 / DOWNSCALE;
		public int TerrainDepthScale = 400 * DOWNSCALE;

		public GeometryBuffers<B> Buffers;

		VertexDeclaration VertexDeclaration
		{
			get { return VertexPositionNormalTexture.VertexDeclaration; }
		}

		protected Terrain(Destiny game, GeometryBuffers<B> buffers)
			: base(game)
		{
			AddChild(Buffers = buffers);
		}

		protected float GetDepth(Sunshine.World.MapTile tile)
		{
			return ((float)tile.Terrain.Depth - Sunshine.World.World.HeightTerrainTypeWater) / TerrainDepthScale;
		}

		protected override void LoadSelf()
		{
			base.LoadSelf();
			var task = Task.Factory.StartNew(() => SetupVertices());
		}

		public override TerrainDebugInfo GetDebugInfo()
		{
			return new TerrainDebugInfo()
			{
				BuffersCount = Buffers.Count,
				//CurrentBuffer = Buffers.GetBuffer().GetDebugInfo(),
				CurrentBuffer = Buffers.ProcessBuffer((buffer) => buffer.GetDebugInfo()),
			};
		}

		protected abstract void SetupVertices();

	}
}

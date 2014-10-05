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

	abstract public class Terrain : VisualElement, ITerrain
	{

		bool Pointing;
		Vector3 LastPointingLocation;
		Vector3 NewPointingLocation;

		public Terrain(Destiny game) : base(game)
		{
		}

		#region ITerrain
		public virtual void SetMapPoint(Vector3 position)
		{
		}

		public void SetPointing(Vector3 position)
		{
			NewPointingLocation = position;
		}

		public void SwitchPointing()
		{
			Pointing = !Pointing;
			NewPointingLocation = Vector3.Zero;
		}

		public abstract TerrainDebugInfo GetDebugInfo();

		#endregion ITerrain

		protected override void UpdateSelf(GameTime gameTime)
		{
			base.UpdateSelf(gameTime);
			if (LastPointingLocation != NewPointingLocation)
			{
				HidePointingLocation();
				if (Pointing)
				{
					ShowPointingLocation(NewPointingLocation);
					LastPointingLocation = NewPointingLocation;

				}
			}
		}

		protected virtual void HidePointingLocation()
		{
		}

		protected virtual void ShowPointingLocation(Vector3 NewPointingLocation)
		{
		}

		protected void SetupRasterization()
		{
			Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			Game.GraphicsDevice.RasterizerState = new RasterizerState()
			{
				CullMode = Game.CullEnabled ? CullMode.CullCounterClockwiseFace : CullMode.None,
				FillMode = Game.SolidEnabled ? FillMode.Solid : FillMode.WireFrame,
			};
		}



	}
	abstract public class Terrain<B> : Terrain
		where B : PolygonBuffer
	{


		public const int DOWNSCALE = 4;
		public int MapTilesWidth = 1024 / DOWNSCALE;
		public int MapTilesHeight = 1024 / DOWNSCALE;
		public int TerrainDepthScale = 400 * DOWNSCALE;

		private GeometryBuffers<B> Buffers;

		VertexDeclaration VertexDeclaration
		{
			get { return VertexPositionNormalTexture.VertexDeclaration; }
		}

		protected Terrain(Destiny game, GeometryBuffers<B> buffers)
			: base(game)
		{
			AddChild(Buffers = buffers);
		}

		protected void ProcessBuffer(Action<B> process)
		{
			Buffers.ProcessBuffer(process);
		}

		protected T ProcessBuffer<T>(Func<B, T> process)
		{
			return Buffers.ProcessBuffer<T>(process);
		}

		protected float GetDepth(Sunshine.World.MapTile tile)
		{
			return ((float)tile.Terrain.Depth - Sunshine.World.World.HeightTerrainTypeWater) / TerrainDepthScale;
		}

		public override TerrainDebugInfo GetDebugInfo()
		{
			return new TerrainDebugInfo()
			{
				BuffersCount = Buffers.Count,
				CurrentBuffer = Buffers.ProcessBuffer((buffer) => buffer.GetDebugInfo()),
			};
		}

		public override void LoadContent()
		{
			base.LoadContent();
			Task.Factory.StartNew(() => SetpVertices());
		}

		public abstract void SetpVertices();

	}
}

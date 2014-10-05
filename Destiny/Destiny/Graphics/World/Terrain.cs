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
	abstract public class Terrain : VisualElement
	{

		public abstract IGeometryBuffers GetBuffers();

		public Terrain(Destiny game) : base(game)
		{
		}

		public virtual void AddCube(Vector3 position)
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

		public GeometryBuffers<B> Buffers;

		VertexDeclaration VertexDeclaration
		{
			get { return VertexPositionNormalTexture.VertexDeclaration; }
		}

		protected Terrain(Destiny game, GeometryBuffers<B> buffers)
			: base(game)
		{
			Buffers = buffers;
		}

		public override IGeometryBuffers GetBuffers()
		{
			return Buffers;
		}


		protected float GetDepth(Sunshine.World.MapTile tile)
		{
			return ((float)tile.Terrain.Depth - Sunshine.World.World.HeightTerrainTypeWater) / TerrainDepthScale;
		}

		public override void LoadContent()
		{
			base.LoadContent();


			Task.Factory.StartNew(() => SetpVertices());
		}

		public abstract void SetpVertices();

	}
}

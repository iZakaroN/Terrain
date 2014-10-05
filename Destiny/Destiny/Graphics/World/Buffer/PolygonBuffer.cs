using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics.World
{
	public class BufferRegion
	{
		private int _low;
		private int _high;
		public BufferRegion(int low, int high)
		{
			_low = low;
			_high = high;
		}

		public int Low { get { return _low; } set { _low = value; } }
		public int High { get { return _high; } set { _high = value; } }

		public int Count
		{
			get { return High - Low; }
		}

	}

	public class PolygonBuffer : VisualElement, IDisposable
	{
		public enum Mode
		{
			Memory,
			Buffered,
			Auto,
		};

		bool Auto = true;
		bool Buffered = false;
		int MaxMeshCount;
		int MaxPolygonCount;

		int PolygonVertexCount;
		int PolygonIndexCount;
		int PolygonPrimitiveCount;

		int MeshPolygons;
		int MeshVertexCount;
		int MeshIndexCount;
		int MeshPrimitiveCount;

		VertexPositionNormalTexture[] Vertices;
		int[] Indices;

		public VertexBuffer _vertexBuffer;
		public IndexBuffer _indexBuffer;

		List<BufferRegion> UsedRegions = new List<BufferRegion>();

		public PolygonBuffer(Destiny game, int maxMeshCount, int meshPolygons, int polygonVertexCount)
			: base(game)
		{
			Initialize(maxMeshCount, meshPolygons, polygonVertexCount);
		}

		public Mode BufferMode
		{
			get
			{
				if (Auto)
					return Mode.Auto;
				if (Buffered)
					return Mode.Buffered;
				return Mode.Memory;

			}
			set
			{
				Auto = false;
				Buffered = false;
				switch (value)
				{
					case Mode.Auto:
						Auto = true;
						goto case Mode.Buffered;
					case Mode.Buffered:
						Buffered = true;
						break;
				}

			}
		}

		public void Initialize(int maxMeshCount, int meshPolygons, int polygonVertexCount)
		{
			MaxMeshCount = maxMeshCount;
			MaxPolygonCount = meshPolygons * maxMeshCount;

			MeshPolygons = meshPolygons;

			PolygonVertexCount = polygonVertexCount;
			PolygonPrimitiveCount = polygonVertexCount - 2;
			PolygonIndexCount = GetIndexCount(polygonVertexCount);

			MeshVertexCount = PolygonVertexCount * meshPolygons;
			MeshPrimitiveCount = PolygonPrimitiveCount * meshPolygons;
			MeshIndexCount = PolygonIndexCount * meshPolygons;

			Vertices = new VertexPositionNormalTexture[MaxPolygonCount * polygonVertexCount];
			Indices = new int[MaxPolygonCount * PolygonIndexCount];
		}

		static int GetIndexCount(int vertexCount) { return (vertexCount - 2) * 3; }

		public int AddMesh(VertexPositionNormalTexture[] vertices, int[] indices)
		{
			var position = AllocateFreePolygonsPosition(MeshPolygons);
			SetPolygons(vertices, indices, position);

			if (Auto && IsFull)
			{
				LoadBufferContent();
				Buffered = true;
				Vertices = null;
				Indices = null;

			}
			return position;
		}

		private void SetPolygons(VertexPositionNormalTexture[] vertices, int[] indices, int position)
		{
			//Debug.Assert(vertices.Count() == MeshVertexCount, "Invalid vertex count.", "Passed {0}. Required {1}.", vertices.Count(), MeshVertexCount);
			//Debug.Assert(indices.Count() == MeshIndexCount, "Invalid index count.", "Passed {0}. Required {1}.", indices.Count(), MeshIndexCount);

			int vertexPosition = position * PolygonVertexCount;
			int indexPosition = position * PolygonIndexCount;

			vertices.CopyTo(Vertices, vertexPosition);
			for (int i = 0; i < MeshIndexCount; i++)
				Indices[indexPosition + i] = vertexPosition + indices[i];
		}

		public void RemoveMesh(int position)
		{

			BufferRegion meshRegion = new BufferRegion(position, position + MeshPolygons);

			//Find containing region
			int regionPosition = 0;
			while (regionPosition < UsedRegions.Count && UsedRegions[regionPosition].High < position) regionPosition++;
			if (regionPosition == UsedRegions.Count || UsedRegions[regionPosition].Low > position)
				throw new KeyNotFoundException("Cannot find proper region"); Debug.Assert(regionPosition != -1, "Cannot find mesh in polygon buffer.", "at position {0}", position);

			Debug.Assert(UsedRegions[regionPosition].Low <= meshRegion.Low, "Invalid mesh region.");
			Debug.Assert(UsedRegions[regionPosition].High >= meshRegion.High, "Invalid mesh region.");

			bool cutLow = UsedRegions[regionPosition].Low == meshRegion.Low;

			bool cutHigh = UsedRegions[regionPosition].High == meshRegion.High;
			if (cutLow && cutHigh)
				UsedRegions.RemoveAt(regionPosition);
			else if (!cutLow)
			{
				if (!cutHigh)
				{
					UsedRegions.Insert(regionPosition + 1, new BufferRegion(meshRegion.High, UsedRegions[regionPosition].High));
				}
				UsedRegions[regionPosition].High = meshRegion.Low;
			}
			else
				UsedRegions[regionPosition].Low = meshRegion.High;
		}

		public int AllocateFreePolygonsPosition(int polygonCount)
		{
			int region = 0;
			int position = 0;

			while (region < UsedRegions.Count() && UsedRegions[region].Low < (position + polygonCount))
				position = UsedRegions[region++].High;

			bool glueUpper = region < UsedRegions.Count() && UsedRegions[region].Low == position + polygonCount;
			if (region > 0)
			{
				if (!glueUpper)
					UsedRegions[region - 1].High = position + polygonCount;
				else
				{
					UsedRegions[region - 1].High = UsedRegions[region].High;
					UsedRegions.RemoveAt(region);
				}
			}
			else if (!glueUpper)
				UsedRegions.Insert(region, new BufferRegion(position, position + polygonCount));
			else
				UsedRegions[region].Low = position;

			return position;
		}

		public BufferDebugInfo GetDebugInfo()
		{
			return new BufferDebugInfo()
			{
				RegionCount = UsedRegions.Count,
				FreePolygons = UsedRegions.Aggregate(MaxPolygonCount, (maxPolygons, region) => maxPolygons - region.Count),
				TotalPolygons = MaxPolygonCount,
			};
		}

		public bool IsFull { get { return UsedRegions.Count == 1 /*> 0*/ && UsedRegions.Last().High >= MaxPolygonCount; } }

		VertexDeclaration VertexDeclaration
		{
			get { return VertexPositionNormalTexture.VertexDeclaration; }
		}

		private void LoadBufferContent()
		{
			var vertexCount = MaxPolygonCount * PolygonVertexCount;
			var indexCount = MaxPolygonCount * PolygonIndexCount;

			_vertexBuffer = new VertexBuffer(Device, VertexDeclaration, vertexCount, BufferUsage.None);
			_vertexBuffer.SetData(Vertices, 0, vertexCount);

			_indexBuffer = new IndexBuffer(Device, typeof(int), indexCount, BufferUsage.None);
			_indexBuffer.SetData(Indices, 0, indexCount);
		}

		protected override void DrawSelf(GameTime gameTime)
		{
			base.DrawSelf(gameTime);
			bool buffered = Buffered;
			if (buffered)
			{
				Device.Indices = _indexBuffer;
				Device.SetVertexBuffer(_vertexBuffer);
			}
			for (int i = 0; i < UsedRegions.Count; i++)
			{
				if (buffered)
					Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, MaxPolygonCount * PolygonVertexCount, UsedRegions[i].Low * PolygonIndexCount, UsedRegions[i].Count * PolygonPrimitiveCount);
				else
					Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, MaxPolygonCount * PolygonVertexCount, Indices, UsedRegions[i].Low * PolygonIndexCount, UsedRegions[i].Count * PolygonPrimitiveCount);
			}
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			if (_vertexBuffer != null) _vertexBuffer.Dispose();
			if (_indexBuffer != null) _indexBuffer.Dispose();
		}
	}
}

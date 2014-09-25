using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sunshine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.World.Square.Mesh
{
	class Tile
	{
		public const int TILE_FACE_COUNT = 1;
		public const int TILE_FACE_VERTICES = 4;
		static private int[] Indices = new int[]
		{
			0 + 4*0, 1 + 4*0, 2 + 4*0, 0 + 4*0, 2 + 4*0, 3 + 4*0,
        };

		private readonly Vector3[] Vectors = new Vector3[TILE_FACE_VERTICES] 
        { 
			new Vector3(0,0,0),new Vector3(1,0,0),new Vector3(1,0,1),new Vector3(0,0,1),
        };

		public static Vector3[] Normals = new Vector3[TILE_FACE_VERTICES] 
        { 
			new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1),
        };

		static private readonly Vector2[] TextureCoordinates = new Vector2[] { 					
			new Vector2(0, 0), 
			new Vector2(1, 0),
			new Vector2(1, 1),
			new Vector2(0, 1),

		};

		TextureTile FaceTexture;
		int BufferPossition;
		TileBuffer Buffer;

		private float GetDepth(Sunshine.World.MapTile tile, int depthScale)
		{
			return ((float)tile.Terrain.Depth - Sunshine.World.World.HeightTerrainTypeWater) / depthScale;
		}

		public Tile(TextureTile faceTextures, MapTile[] tiles, int depthScale)
		{
			FaceTexture = faceTextures;
			for (int i = 0; i < TILE_FACE_VERTICES; i++)
				Vectors[i].Y = GetDepth(tiles[i], depthScale);
			for (int i = 0; i < TILE_FACE_VERTICES; i++)
			{
				Normals[i] = new Vector3((float)(tiles[i].Data.A - tiles[i].Data.C) / depthScale, (float)(tiles[i].Data.B - tiles[i].Data.D) / depthScale, 2);
				Normals[i].Normalize();
			}
		}

		public void AddToBuffer(TileBuffer buffer, Vector3 position)
		{
			Buffer = buffer;
			BufferPossition = buffer.AddMesh(GetVertices(position, FaceTexture), Indices);
		}

		private VertexPositionNormalTexture[] GetVertices(Vector3 position, TextureTile texture)
		{
			var vertices = new VertexPositionNormalTexture[TILE_FACE_VERTICES];

			for (int i = 0; i < TILE_FACE_VERTICES; i++)
			{
				vertices[i].Position = position + Vectors[i];
				vertices[i].Normal = Normals[i];
				vertices[i].TextureCoordinate = texture.GetPosition(new Vector2(vertices[i].Position.X, vertices[i].Position.Z));
			}
			float xLength = Math.Abs((vertices[1].Position - vertices[0].Position).Length());
			float yLength = Math.Abs((vertices[3].Position - vertices[0].Position).Length());
			float dxLength = Math.Abs((vertices[2].Position - vertices[3].Position).Length());
			float dyLength = Math.Abs((vertices[2].Position - vertices[1].Position).Length());

			var start = new Vector2(vertices[0].Position.X, vertices[0].Position.Z);
			/*vertices[0].TextureCoordinate = texture.GetPosition(start + new Vector2(0, 0));
			vertices[1].TextureCoordinate = texture.GetPosition(start + new Vector2(xLength, 0));
			vertices[2].TextureCoordinate = texture.GetPosition(start + new Vector2(dxLength, dyLength));
			//vertices[2].TextureCoordinate = texture.GetPosition(start + new Vector2(xLength, yLength));
			vertices[3].TextureCoordinate = texture.GetPosition(start + new Vector2(0, yLength));*/
			vertices[0].TextureCoordinate = texture.GetPosition(start + new Vector2(0, 0));
			vertices[1].TextureCoordinate = texture.GetPosition(start + new Vector2(1, 0));
			vertices[2].TextureCoordinate = texture.GetPosition(start + new Vector2(1, 1));
			vertices[3].TextureCoordinate = texture.GetPosition(start + new Vector2(0, 1));
			return vertices;
		}

		private static Vector2 GetTextureCoordinates(TextureTile texture, VertexPositionNormalTexture distance)
		{
			return texture.GetPosition(new Vector2(distance.Position.X, distance.Position.Z));
		}



	}
}

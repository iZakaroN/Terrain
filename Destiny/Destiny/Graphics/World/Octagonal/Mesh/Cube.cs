using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Destiny.Graphics.World;
using Destiny.Graphics;
using Destiny.Graphics.World.Octagonal.Dimension;

namespace Destiny.Graphics.World.Octagonal.Mesh
{
    public class Cube : OctantNode
	{

		public const int CUBE_FACE_COUNT = 6;
		public const int CUBE_FACE_VERTICES = 4;

		public const int CUBE_VERTICES_COUNT = CUBE_FACE_COUNT * CUBE_FACE_VERTICES;
		public const int CUBE_INDEX_COUNT = 36;
		public const int CUBE_PRIMITIVE_COUNT = 12;

		static private int[] Indices = new int[]
//		static private int[] FaceIndices = new int[]
			{
				0 + 4*0, 1 + 4*0, 2 + 4*0, 0 + 4*0, 2 + 4*0, 3 + 4*0,
				0 + 4*1, 1 + 4*1, 2 + 4*1, 0 + 4*1, 2 + 4*1, 3 + 4*1,
				0 + 4*2, 1 + 4*2, 2 + 4*2, 0 + 4*2, 2 + 4*2, 3 + 4*2,
				0 + 4*3, 1 + 4*3, 2 + 4*3, 0 + 4*3, 2 + 4*3, 3 + 4*3,
				0 + 4*4, 1 + 4*4, 2 + 4*4, 0 + 4*4, 2 + 4*4, 3 + 4*4,
				0 + 4*5, 1 + 4*5, 2 + 4*5, 0 + 4*5, 2 + 4*5, 3 + 4*5,
			};

		static private readonly Vector3[] Vectors = new Vector3[CUBE_VERTICES_COUNT] { 
			new Vector3(0,1,0),new Vector3(1,1,0),new Vector3(1,1,1),new Vector3(0,1,1), //Top
			new Vector3(0,0,1),new Vector3(1,0,1),new Vector3(1,0,0),new Vector3(0,0,0), //Bottom

			new Vector3(0,1,0),new Vector3(0,1,1),new Vector3(0,0,1),new Vector3(0,0,0),//Left
			new Vector3(1,1,1),new Vector3(1,1,0),new Vector3(1,0,0),new Vector3(1,0,1),//Right

			new Vector3(0,1,1),new Vector3(1,1,1),new Vector3(1,0,1),new Vector3(0,0,1),//Front
			new Vector3(1,1,0),new Vector3(0,1,0),new Vector3(0,0,0),new Vector3(1,0,0),//Back

		};

		static public readonly Vector3[] VectorsLineList = new Vector3[24] { 
			new Vector3(0,0,0),new Vector3(1,0,0),
			new Vector3(0,0,0),new Vector3(0,1,0),
			new Vector3(0,0,0),new Vector3(0,0,1),

			new Vector3(1,1,1),new Vector3(0,1,1),
			new Vector3(1,1,1),new Vector3(1,0,1),
			new Vector3(1,1,1),new Vector3(1,1,0),

			new Vector3(1,0,0),new Vector3(1,1,0),
			new Vector3(1,0,0),new Vector3(1,0,1),

			new Vector3(0,1,0),new Vector3(1,1,0),
			new Vector3(0,1,0),new Vector3(0,1,1),

			new Vector3(0,0,1),new Vector3(1,0,1),
			new Vector3(0,0,1),new Vector3(0,1,1),

		};


		static private readonly Vector3[] Normal = new Vector3[CUBE_VERTICES_COUNT]
			{
				new Vector3(0,1,0),new Vector3(0,1,0),new Vector3(0,1,0),new Vector3(0,1,0), //Top
				new Vector3(0,-1,0),new Vector3(0,-1,0),new Vector3(0,-1,0),new Vector3(0,-1,0), //Bottom
				new Vector3(-1,0,0),new Vector3(-1,0,0),new Vector3(-1,0,0),new Vector3(-1,0,0),//Left
				new Vector3(1,0,0),new Vector3(1,0,0),new Vector3(1,0,0),new Vector3(1,0,0),//Right
				new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1),new Vector3(0,0,1),//Front
				new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(0,0,-1),//Back
			};

		static private readonly Vector2[] TextureCoordinates = new Vector2[] { 					
			new Vector2(0, 0), 
			new Vector2(1, 0),
			new Vector2(1, 1),
			new Vector2(0, 1),

		};

		List<TextureTile> FacesTexture;
//		Vector3 Position;
		int BufferPossition;
		CubeBuffer Buffer;


		public Cube(List<TextureTile> faceTextures)
		{
			FacesTexture = faceTextures;
		}

		override public OctantNode Node { get { return this; } }

		public void AddToBuffer(CubeBuffer buffer, Vector3 position)
		{
			Buffer = buffer;
			BufferPossition = buffer.AddMesh(GetVertices(position, FacesTexture), Indices);
		}

		internal void RemoveFromBuffer()
		{
			Buffer.RemoveMesh(BufferPossition);
		}


		/*private VertexPositionNormalTexture[] GetVertices()
		{
			return GetVertices(Position, FacesTexture);
		}*/

		static private VertexPositionNormalTexture[] GetVertices(Vector3 position, List<TextureTile> textures)
		{
			var vertices = new VertexPositionNormalTexture[CUBE_VERTICES_COUNT];
			int face = -1;
			int faceVertex = CUBE_FACE_VERTICES - 1;
//			int faceRotation = 0;
			TextureTile texture = null;

			for (int i = 0; i < CUBE_VERTICES_COUNT; i++)
			{
				if (++faceVertex == CUBE_FACE_VERTICES)
				{
					texture = textures[++face];
					faceVertex = 0;
//					faceRotation = FaceTextureRotation[face];
				}
				var cornerVector = Vectors[i];
				vertices[i].Position = position + cornerVector;
				vertices[i].Normal = Normal[i];
				vertices[i].TextureCoordinate = texture.GetPosition(TextureCoordinates[faceVertex]);

			}
			return vertices;
		}

		//public const int CUBE_VERTICES_COUNT = 8;
		//public const int CUBE_INDEX_COUNT = 14;
		//public const int CUBE_PRIMITIVE_COUNT = 12;
		//static public int[] Indices = new int[14] { 0, 1, 2, 3, 7, 1, 5, 0, 4, 2, 6, 7, 4, 5 };

		//static public readonly Vector3[] ZeroCube = new Vector3[]
		//{
		//    new Vector3(-1, -1, -1),
		//    new Vector3( 1, -1, -1),
		//    new Vector3(-1,  1, -1),
		//    new Vector3( 1,  1, -1),

		//    new Vector3(-1, -1,  1),
		//    new Vector3( 1, -1,  1),
		//    new Vector3(-1,  1,  1),
		//    new Vector3( 1,  1,  1),
		//};

		//static public readonly Vector3[] Normal = new Vector3[]
		//{
		//    new Vector3(-1, -1, -1),
		//    new Vector3( 1, -1, -1),
		//    new Vector3(-1,  1, -1),
		//    new Vector3( 1,  1, -1),

		//    new Vector3(-1, -1,  1),
		//    new Vector3( 1, -1,  1),
		//    new Vector3(-1,  1,  1),
		//    new Vector3( 1,  1,  1),
		//};


		//static public readonly Vector2[] TextureCoordinates = new Vector2[]
		//{
		//    new Vector2(1, 0),
		//    new Vector2(0, 0),
		//    new Vector2(1, 1),
		//    new Vector2(0, 1),

		//    new Vector2(0, 1),
		//    new Vector2(1, 1),
		//    new Vector2(0, 0),
		//    new Vector2(1, 0),
		//};
		static Cube()
		{
/*			Indices = new int[CUBE_INDEX_COUNT];
			Vectors = new Vector3[CUBE_VERTICES_COUNT];
			Normal = new Vector3[CUBE_VERTICES_COUNT];
			var baseVertex = 0;
			var baseIndex = 0;

			for (int axisMask = 1; axisMask < CubicVector.MASK_ALL; axisMask <<= 1)
			{
				int vector = 0;
				for (int i = 0; i < 2; i++)
				{
					//					vector = CubicVector.AxisMaskShift(CubicVector.AxisMaskShift(axisMask)) | vector;
					var maskN2 = CubicVector.AxisMaskShift(axisMask);
					var maskN1 = CubicVector.AxisMaskShift(maskN2);

					var vN0 = new CubicVector(vector);
					var vN1 = vN0.Oposite(maskN1);
					var vN2 = vN0.Oposite(maskN2);
					var vN3 = vN1.Oposite(maskN2);
					var normal = new CubicVector(axisMask).Vector3 * (i * 2 - 1);
					var zeroVector = new Vector3(0.5f, 0.5f, 0.5f);
					normal.Normalize();


					//No index buffer
					//TextureCoordinates[baseVertex] = new Vector2(0, 0); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN0.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(0, 1); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN1.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 0); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN2.Vector3;

					//TextureCoordinates[baseVertex] = new Vector2(1, 0); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN2.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(0, 1); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN1.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 1); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN3.Vector3;

					//Face normals
					//TextureCoordinates[baseVertex] = new Vector2(0, 0); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN0.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(0, 1); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN1.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 0); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN2.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 1); Normal[baseVertex] = normal; Vectors[baseVertex++] = vN3.Vector3;

					Normal[baseVertex] = normal; Vectors[baseVertex++] = vN0.Vector3;
					Normal[baseVertex] = normal; Vectors[baseVertex++] = vN1.Vector3;
					Normal[baseVertex] = normal; Vectors[baseVertex++] = vN2.Vector3;
					Normal[baseVertex] = normal; Vectors[baseVertex++] = vN3.Vector3;
					//Average normals
					//TextureCoordinates[baseVertex] = new Vector2(0, 0); Normal[baseVertex] = vN0.Vector3 - zeroVector; Vectors[baseVertex++] = vN0.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(0, 1); Normal[baseVertex] = vN1.Vector3 - zeroVector; Vectors[baseVertex++] = vN1.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 0); Normal[baseVertex] = vN2.Vector3 - zeroVector; Vectors[baseVertex++] = vN2.Vector3;
					//TextureCoordinates[baseVertex] = new Vector2(1, 1); Normal[baseVertex] = vN3.Vector3 - zeroVector; Vectors[baseVertex++] = vN3.Vector3;


					Indices[baseIndex++] = baseVertex + 0 - 4;
					Indices[baseIndex++] = baseVertex + 1 - 4;
					Indices[baseIndex++] = baseVertex + 2 - 4;

					Indices[baseIndex++] = baseVertex + 2 - 4;
					Indices[baseIndex++] = baseVertex + 1 - 4;
					Indices[baseIndex++] = baseVertex + 3 - 4;
					vector += CubicVector.AxisMaskShift(axisMask) | axisMask;
				}
			}*/
		}
	}
}

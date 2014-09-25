//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Destiny.Graphics;

//namespace Destiny
//{
//    public class Terrain : VisualElement
//    {
//        TextureTile _texture;

//        VertexPositionNormalTexture[] Vertices;
//        int[] Indices;

//        public const int MAX_CUBECOUNT = 40 * 30 * 30;
//        int buffers = 0;
//        public VertexBuffer[] _vertexBuffer;
//        public IndexBuffer[] _indexBuffer;

//        public Terrain(Destiny game)
//            : base(game)
//        {
//        }

//        VertexDeclaration VertexDeclaration
//        {
//            get { return VertexPositionNormalTexture.VertexDeclaration; }
//        }

//        int _xCount = 500;
//        int _yCount = 1;
//        int _zCount = 500;

//        int CubeCount()
//        {
//            return _xCount * _yCount * _zCount * 2;
//        }

//        void CubeIterrator(Action<Vector3> iterrator)
//        {
//            Vector3 cubeVector = new Vector3(0, 0, 0);
//            for (cubeVector.X = 0; cubeVector.X < _xCount; cubeVector.X++)
//                for (cubeVector.Y = 0; cubeVector.Y < _yCount; cubeVector.Y++)
//                    for (cubeVector.Z = 0; cubeVector.Z < _zCount; cubeVector.Z++)
//                    {
//                        iterrator(cubeVector);
//                        iterrator(-Vector3.One - cubeVector);
//                    }
//        }

//        void SetUpVertices()
//        {
//            Vertices = new VertexPositionNormalTexture[CubeCount() * Cube.CUBE_VERTICES_COUNT];
//            Indices = new int[CubeCount() * Cube.CUBE_INDEX_COUNT];
//            int vertexIndex = 0;
//            int indiceIndex = 0;
//            CubeIterrator((vector) =>
//                {
//                    Cube.GetVertices(vector * 4, _texture).CopyTo(Vertices, vertexIndex);
//                    Cube.Indices.CopyTo(Indices, indiceIndex);
//                    for (int i = 0; i < Cube.CUBE_INDEX_COUNT; i++)
//                        Indices[i + indiceIndex] += vertexIndex;
//                    vertexIndex += Cube.CUBE_VERTICES_COUNT;
//                    indiceIndex += Cube.CUBE_INDEX_COUNT;
//                });
//        }

//        override public void LoadContent()
//        {
//            //			_texture = Content.Load<Texture2D>(@"sample");
//            _texture = new TextureTile(Content.Load<Texture2D>(@"Textures\terrain"), 4, 2);
//            //			_texture = Content.Load<Texture2D>(@"Textures\free-grass-texture");

//            SetUpVertices();
//            buffers = (CubeCount() - 1) / MAX_CUBECOUNT + 1;

//            _vertexBuffer = new VertexBuffer[buffers];
//            _indexBuffer = new IndexBuffer[buffers];
//            var vertexCount = Cube.CUBE_VERTICES_COUNT * MAX_CUBECOUNT;
//            var indexCount = Cube.CUBE_INDEX_COUNT * MAX_CUBECOUNT;
//            int vertexPos = 0;
//            int indexPos = 0;
//            for (int i = 0; i < buffers; i++)
//            {
//                if (vertexPos + vertexCount > Vertices.Length)
//                    vertexCount = Vertices.Length - vertexPos;
//                if (indexPos + indexCount > Indices.Length)
//                    indexCount = Indices.Length - indexPos;
//                _vertexBuffer[i] = new VertexBuffer(Device, VertexDeclaration, vertexCount, BufferUsage.WriteOnly);
//                _vertexBuffer[i].SetData(Vertices, vertexPos, vertexCount);

//                for (int j = 0; j < indexCount; j++)
//                    Indices[j + indexPos] -= vertexPos;

//                _indexBuffer[i] = new IndexBuffer(Device, typeof(int), indexCount, BufferUsage.WriteOnly);
//                _indexBuffer[i].SetData(Indices, indexPos, indexCount);
//                vertexPos += vertexCount;
//                indexPos += indexCount;
//            }
//            base.LoadContent();
//        }

//        override public void Draw()
//        {
//            Effect.Parameters["xTexture"].SetValue(_texture.Texture);
//            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
//            {
//                pass.Apply();
//                int vertexIndex = 0;
//                int indiceIndex = 0;

//                int cubeCount = MAX_CUBECOUNT;
//                int totalCubes = 0;

//                for (int i = 0; i < buffers; i++)
//                {
//                    if (cubeCount + totalCubes > CubeCount())
//                        cubeCount = CubeCount() - totalCubes;
//                    Device.Indices = _indexBuffer[i];
//                    Device.SetVertexBuffer(_vertexBuffer[i]);
//                    Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Cube.CUBE_VERTICES_COUNT * cubeCount, 0, Cube.CUBE_PRIMITIVE_COUNT * cubeCount);
//                    //					Device.DrawPrimitives(PrimitiveType.TriangleList, 0, Cube.CUBE_PRIMITIVE_COUNT * cubeCount);
//                    totalCubes += cubeCount;
//                }
//                //				Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Cube.CUBE_VERTICES_COUNT * CubeCount(), Indices, 0, Cube.CUBE_PRIMITIVE_COUNT * CubeCount(), VertexDeclaration);
//                /*				for (int i = 0; i < CubeCount(); i++)
//                                {
//                                    Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, vertexIndex, Cube.CUBE_VERTICES_COUNT, Indices, indiceIndex, Cube.CUBE_PRIMITIVE_COUNT, VertexDeclaration);
//                //					Device.DrawUserIndexedPrimitives(PrimitiveType.TriangleStrip, Vertices, vertexIndex, Cube.CUBE_VERTICES_COUNT, Indices, indiceIndex, Cube.CUBE_PRIMITIVE_COUNT, VertexDeclaration);
//                //					Device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, vertexIndex, 0, Cube.CUBE_VERTICES_COUNT, indiceIndex, Cube.CUBE_PRIMITIVE_COUNT);
//                                    vertexIndex += Cube.CUBE_VERTICES_COUNT;
//                                    indiceIndex += Cube.CUBE_INDEX_COUNT;
//                                }*/
//            }
//            base.Draw();
//        }


//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Destiny.Graphics.World.Octagonal.Dimension
{
	public class Dimension //: VisualElement
	{
		public OctagonalVector _rootVector;
		public Octant _root = null;
		public int _depth = 0;

		public class Iterator
		{
			Action<Octant, List<int>> _action;
			List<int> _path;
			Dimension _dimension;

			public Iterator(Dimension dimension, Action<Octant, List<int>> action)
			{
				_action = action;
				_dimension = dimension;
			}

			public void Iterrate()
			{
				_path = new List<int>();
				Itterate(_dimension._root);
			}

			private void Itterate(Octant octant)
			{
				if (octant!=null)
				{
					if (octant.HasChilds)
					{
						_path.Insert(0, 0);
						for (int i = 0; i <= 7; i++)
						{
							_path[0] = i;
							Itterate(octant[i]);
						}
						_path.RemoveAt(0);
					}
					_action(octant, _path);
				}
			}

		}

		public Iterator GetIterrator(Action<Octant, List<int>> action)
		{
			return new Iterator(this, action);
		}

		public OctagonalVector AllocateNode(Vector3 v, OctantNode node)
		{
			return AllocateNode(new OctagonalVector(v), node);
		}

        public OctagonalVector AllocateNode(OctagonalVector v, OctantNode node)
		{
			lock (this)
			{
				if (_root == null)
				{
					_rootVector = v;
					_depth = 0;
					_root = node;
					return new OctagonalVector(0, 0, 0);
				}

				AllocateLowerBoundRoot(v);

				OctagonalVector dV = v - _rootVector;

				AllocateVector(dV.CalculatePath(), node);

				return dV;
			}
		}

		private void AllocateLowerBoundRoot(OctagonalVector v)
		{
			OctagonalVector dV = v - _rootVector;

			OctagonalVector oldRoot = new OctagonalVector(
				(dV.X < 0) ? ((-dV.X - 1) >> _depth) + 1 : 0,
				(dV.Y < 0) ? ((-dV.Y - 1) >> _depth) + 1 : 0,
				(dV.Z < 0) ? ((-dV.Z - 1) >> _depth) + 1 : 0);


			List<int> oldRootPath = oldRoot.CalculatePath();
			int oldRootPathLength = oldRootPath.Count();
			if (oldRootPathLength > 0)
			{
				for (int i = 0; i < oldRootPathLength; i++)
					_root = new Octant(oldRootPath[i], _root);

				_rootVector -= oldRoot << _depth;
				_depth += oldRootPathLength;
			}
		}
		void AllocateVector(List<int> vPath, OctantNode node)
		{
			int vectorPosition = vPath.Count();

			//AllocateUpperBoundRoot
			while (_depth < vPath.Count)
			{
				_root = new Octant(0, _root); _depth++;
			}

			//Find Octant
			Octant current = _root;

			//Skip missing (zero Octant) path coordinates
			int vectorDimension = _depth-1;
			while (vectorDimension > 0 && vectorPosition <= vectorDimension)
			{
				current = current.GetOrAllocate(0); vectorDimension--;
			}

			//Find the path
			while (vectorDimension > 0)
				current = current.GetOrAllocate(vPath[vectorDimension--]);

			if (vectorPosition>0)
				current[vPath[0]] = node;
			else
				current[0] = node;
		}

		int Min(int a, int b, int c)
		{
			return Math.Min(Math.Min(a, b),c);
		}

		int Min(int a, int b)
		{
			return Math.Min(a, b);
		}

		int Max(int a, int b)
		{
			return Math.Max(a, b);
		}

		internal Vector3 ToWorldVector(OctagonalVector oVector)
		{
			return oVector + _rootVector;
		}

		public VertexPositionColor[] GetPathLines(List<int> path, Color color)
		{
			int size = 1 << (_depth - path.Count);
			var vector = OctagonalVector.CalculateVector(path) * size + _rootVector;

			var result = new VertexPositionColor[24];
			for (int i = 0; i < 24; i++)
			{
				result[i].Color = color;
				result[i].Position = Octagonal.Mesh.Cube.VectorsLineList[i] * size + (Vector3)vector;
			}
			return result;
		}

	}
}

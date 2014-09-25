using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Destiny.Graphics.World.Octagonal.Dimension
{
	public struct OctagonalVector 
	{
		public const int BIT_X = 0;
		public const int BIT_Y = 1;
		public const int BIT_Z = 2;

		public int X;
		public int Y;
		public int Z;

		public OctagonalVector(int x, int y, int z)
		{

			X = x;
			Y = y;
			Z = z;
		}

		public OctagonalVector(Vector3 vector3)
		{

			X = (int)vector3.X;
			Y = (int)vector3.Y;
			Z = (int)vector3.Z;
		}

		public static OctagonalVector operator -(OctagonalVector v1, OctagonalVector v2)
		{
			return new OctagonalVector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		public static OctagonalVector operator +(OctagonalVector v1, OctagonalVector v2)
		{
			return new OctagonalVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		public static OctagonalVector operator *(OctagonalVector v1, int v)
		{
			return new OctagonalVector(v1.X * v, v1.Y * v, v1.Z * v);
		}

		public static OctagonalVector operator <<(OctagonalVector v1, int v2)
		{
			return new OctagonalVector(v1.X << v2, v1.Y << v2, v1.Z << v2);
		}

		public static OctagonalVector operator >>(OctagonalVector v1, int v2)
		{
			return new OctagonalVector(v1.X >> v2, v1.Y >> v2, v1.Z >> v2);
		}

		public static implicit operator OctagonalVector(Vector3 v)
		{
			return new OctagonalVector(v);
		}

		public static implicit operator Vector3(OctagonalVector v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		public List<int> CalculatePath()
		{
			List<int> path = new List<int>();
			int bit = 1;
			int pos = 0;
			while (bit <= (X | Y | Z)) //Are there are more bits left?
			{
				var child = (((X & bit) >> pos) << BIT_X) | (((Y & bit) >> pos) << BIT_Y) | (((Z & bit) >> pos) << BIT_Z);
				Debug.Assert(child >=0 &&  child <= 7);
				path.Add(child);
				bit <<= 1; pos++;
			}
			return path;
		}

		static public OctagonalVector CalculateVector(List<int> path)
		{
			int pathLength = path.Count;
			int bit = 0;
			OctagonalVector v = new OctagonalVector(0,0,0);
			while (bit<pathLength)
			{
				v.X |= ((path[bit] & (1 << BIT_X)) >> BIT_X) << bit;
				v.Y |= ((path[bit] & (1 << BIT_Y)) >> BIT_Y) << bit;
				v.Z |= ((path[bit] & (1 << BIT_Z)) >> BIT_Z) << bit;
				bit++;
			}
			return v;
		}


		public override string ToString()
		{
			return string.Format("({0},{1},{2})", X, Y, Z);
		}
	}
}

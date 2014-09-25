using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics
{
	public struct CubicVector
	{
		public const int AXIS_X = 0;
		public const int AXIS_Y = 1;
		public const int AXIS_Z = 2;

		public const int MASK_X = 1 << AXIS_X;
		public const int MASK_Y = 1 << AXIS_Y;
		public const int MASK_Z = 1 << AXIS_Z;

		public const int MASK_ALL = MASK_X | MASK_Y | MASK_Z;

		public const int AXIS_COUNT = 3;

		public const int DIRECTION_COUNT = 2;

		private int Vector;

		public CubicVector(int vector)
		{
			Vector = vector;
		}

		public CubicVector(int axis, int direction)
		{
			Vector = 0;
			this[AxisMask(axis)] = direction;
		}

		public int X { get { return this[MASK_X] != 0 ? 1 : 0; } }
		public int Y { get { return this[MASK_Y] != 0 ? 1 : 0; } }
		public int Z { get { return this[MASK_Z] != 0 ? 1 : 0; } }

		public CubicVector Oposite(int axisMask)
		{
			var result = new CubicVector(Vector);
			result[axisMask] = ~result[axisMask];
			return result;
		}

		public CubicVector Merge(int axisMask)
		{
			return new CubicVector(Vector | axisMask);
		}

		internal CubicVector Merge(CubicVector vector)
		{
			return new CubicVector(Vector | vector.Vector);
		}

		public Vector3 Vector3 { get { return new Vector3(X, Y, Z); } }

		public int this[int axisMask]
		{
			get { return (Vector & axisMask)/* !=0?1:0*/; }
			set { Vector = ((Vector & ~axisMask) | (value & axisMask/*<< axisMask*/)); }
		}

		public static int AxisMask(int axis)
		{
			return (1 << axis);
		}

		public static int AxisMaskShift(int axisMask)
		{
			return MASK_ALL & (axisMask << 1 | ((axisMask >> 2) & 1));
		}

		public static int AxisR(int axis)
		{
			return ((axis + 1) % 3);
		}

		public static int AxisL(int axis)
		{
			return ((axis + 2) % 3);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1};{2};{3}", Vector, X, Y, Z);
		}

	}
}

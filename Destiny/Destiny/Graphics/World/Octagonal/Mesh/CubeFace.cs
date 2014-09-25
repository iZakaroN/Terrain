using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics.World.Octagonal.Mesh
{
	public class CubeFace
	{

		TextureTile TextureTile;
		public CubeFace(TextureTile tile)
		{
			TextureTile = tile;
		}

		public static CubicVector FaceNormal(int axis, int direction)
		{
			return new CubicVector(axis, direction);
		}

		public static CubicVector FaceNormal(int face)
		{
			return new CubicVector(FaceAxis(face), FaceDirection(face));
		}

		public static int FaceDirection(int face)
		{
			return face & 1;
		}

		public static int FaceAxis(int face)
		{
			return face >> 1;
		}

		public static int FaceMask(int face)
		{
			return 1 << FaceDirection(face);
		}

		public static int FaceR(int face)
		{
			return (face + 1) % 3;
		}
		public static int FaceL(int face)
		{
			return (face + 2) % 3;
		}
	}
}

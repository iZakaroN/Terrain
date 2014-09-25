using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics
{
	public class TextureTile
	{
		public int _tileSize;
		public Texture2D Texture;
		double _offsetX;
		double _offsetY;
		double _tileWidth;
		double _tileHeight;

		public TextureTile(Texture2D texture, int indexX, int indexY) : this(texture, 32, indexX, indexY)
		{
		}

		public TextureTile(Texture2D texture,int tileSize , int indexX, int indexY)
		{
			Texture = texture;
			_tileSize = tileSize;
			_tileWidth = (double)_tileSize / texture.Width;
			_tileHeight = (double)_tileSize / texture.Height;
			_offsetX = (double)indexX * _tileWidth;
			_offsetY = (double)indexY * _tileHeight;
		}

		public Vector2 GetPosition(Vector2 relPos)
		{
			return new Vector2((float)(_offsetX + relPos.X * _tileWidth), (float)(_offsetY + relPos.Y * _tileHeight));
		}
	}
}

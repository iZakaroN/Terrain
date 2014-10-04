using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics
{
	public class Crosshair : VisualElement, IDisposable
	{
		SpriteBatch _spriteBatch;
		Texture2D _texture;

		public Crosshair(Destiny game)
			: base(game)
		{
		}

		protected override void LoadSelf()
		{
			_spriteBatch = new SpriteBatch(Device);
			_texture = Content.Load<Texture2D>(@"Textures\Crosshair\crosshair1Small3");
		}

		protected override void DrawSelf()
		{
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);//, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
			_spriteBatch.Draw(_texture, Position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
			_spriteBatch.End();
		}

		Rectangle Position
		{
			get
			{
				var pos = Size;
				pos.Offset(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
				return pos;
			}
		}

		Rectangle Size
		{
			get
			{
				return new Rectangle(-_texture.Width / 2, -_texture.Height / 2, _texture.Width / 2, _texture.Height / 2);
			}
		}

		protected override void UnloadSelf()
		{
			_spriteBatch.Dispose();
			_texture.Dispose();
		}
	}
}

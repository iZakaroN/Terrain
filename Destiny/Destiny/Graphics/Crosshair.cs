using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Destiny.Graphics
{
	public class Crosshair : VisualElement
	{
		SpriteBatch _spriteBatch;
		Texture2D _texture;

		public Crosshair(Destiny game)
			: base(game)
		{
		}

		override public void LoadContent()
		{
			_spriteBatch = new SpriteBatch(Device);
			_texture = Content.Load<Texture2D>(@"Textures\Crosshair\crosshair1Small3");
			
		}

        override public void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);//, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
			_spriteBatch.Draw(_texture, Position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
			_spriteBatch.End();
			base.Draw(gameTime);
		}

/*		public readonly Vector3[] Crosshair = new Vector3[]
        {
            new Vector3(-1, -1, -1),
            new Vector3(1, -1, -1),
            new Vector3(-1, 1, -1),
            new Vector3(1, 1, -1),

            new Vector3(-1, -1, 1),
            new Vector3(1, -1, 1),
            new Vector3(-1, 1, 1),
            new Vector3(1, 1, 1),
        };*/

		Rectangle Position
		{
			get
			{
/*				return new Vector2(
					(Game.Window.ClientBounds.Width - _texture.Width ) / 2,
					(Game.Window.ClientBounds.Height - _texture.Height) / 2);*/
				var pos = Size;
				pos.Offset(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height/2);
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

	}
}

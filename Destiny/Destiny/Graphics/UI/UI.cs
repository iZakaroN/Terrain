using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Destiny.Input;
using Microsoft.Xna.Framework.Input;

namespace Destiny.Graphics.UI
{
	public class TextElement
	{
		public string Text;
		public List<Func<object>> Arguments;

		internal string GetFormatedString()
		{
			return string.Format(Text, Arguments.ConvertAll((arg) => arg()).ToArray());
		}
	}

	public class BaseUI : VisualElement
	{
		public const string TEST_TEXT = "Velizar";

		public bool Enabled = true;

		SpriteBatch _spriteBatch;
		Texture2D _backgroundTexture;
		SpriteFont _font;

		protected Rectangle BackgroundArea = new Rectangle(0,0,200,100);

		Vector2 _spritePosition = Vector2.Zero;

//		Vector2 _spriteSpeed = Vector2.One * 100;
//		Vector2 _textSize = Vector2.One * 100;

		string _textureName;
		string _fontName;

		List<TextElement> _textElements = new List<TextElement>();

		public BaseUI(Destiny game, string fontName, string textureName)
			: base(game)
		{
			_textureName = textureName;
			_fontName = fontName;
		}

		public void AddText(string text, params Func<object>[] arguments)
		{
			_textElements.Add(new TextElement() { Text = text, Arguments = arguments.ToList() });
		}

        public void ClearText()
        {
            _textElements.Clear();
        }

        override public void Update(GameTime gameTime)
		{
			// Move the sprite by speed, scaled by elapsed time.
/*			_spritePosition += _spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			int MaxX = Device.Viewport.Width - (int)_textSize.X;
			int MinX = 0;
			int MaxY = Device.Viewport.Height - (int)_textSize.Y;
			int MinY = 0;

			// Check for bounce.
			if (_spritePosition.X > MaxX)
			{
				_spriteSpeed.X *= -1;
				_spritePosition.X = MaxX;
			}
			else if (_spritePosition.X < MinX)
			{
				_spriteSpeed.X *= -1;
				_spritePosition.X = MinX;
			}

			if (_spritePosition.Y > MaxY)
			{
				_spriteSpeed.Y *= -1;
				_spritePosition.Y = MaxY;
			}
			else if (_spritePosition.Y < MinY)
			{
				_spriteSpeed.Y *= -1;
				_spritePosition.Y = MinY;
			}*/
		}

		protected Vector2 Position
		{
			get { return _spritePosition; }
			set { _spritePosition = value; }
		}

		override public void LoadContent()
		{
			_spriteBatch = new SpriteBatch(Device);
			if (_fontName!=null)
				_font = Content.Load<SpriteFont>(_fontName);
			if (_textureName!=null)
				_backgroundTexture = Content.Load<Texture2D>(_textureName);
//			_textSize = _font.MeasureString(TEST_TEXT);
		}

        override public void Draw(GameTime gameTime)
		{
			if (Enabled)
			{
				_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
				if (_backgroundTexture!=null)
					DrawBackground(_spriteBatch);
				if (_font != null)
					DrawText(_spriteBatch);
				_spriteBatch.End();
			}
		}

		private void DrawBackground(SpriteBatch spriteBatch)
		{
			int x = 0;
			int width = _backgroundTexture.Width;
			bool flipX = false;

			while (x < BackgroundArea.Width)
			{
				if ((x + width) > BackgroundArea.Width)
					width = BackgroundArea.Width - x;

				int y = 0;
				int height = _backgroundTexture.Height;
				bool flipY = false;

				while (y < BackgroundArea.Height)
				{
					if ((y + height) > BackgroundArea.Height)
						height = BackgroundArea.Height - y;
					spriteBatch.Draw(_backgroundTexture, new Rectangle(x, y, width, height), 
						new Rectangle(flipX ? (_backgroundTexture.Width - width) : 0, flipY ? (_backgroundTexture.Height - height) : 0, width, height), Color.White, 0,
						Vector2.Zero, (flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipY ? SpriteEffects.FlipVertically: SpriteEffects.None), 0);
					y += height;
					flipY = !flipY;
				}
				x += width;
				flipX = !flipX;
			}
		}

		private void DrawText(SpriteBatch spriteBatch)
		{
            Vector2 offset = new Vector2(0,0);
            _textElements.ForEach((element) =>
				{
                    var s = element.GetFormatedString();
                    spriteBatch.DrawString(_font, s, _spritePosition + offset, Color.White);
                    offset += new Vector2(0, _font.MeasureString(s).Y + 2);
				});
		}

	}
}

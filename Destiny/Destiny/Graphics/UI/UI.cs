using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Destiny.Input;
using Destiny.Extensions;

namespace Destiny.Graphics.UI
{
    public class TextElement
    {
        public string Text;
        public List<Func<object>> Arguments;

        internal string GetFormatedText()
        {
            return string.Format(Text, Arguments.ConvertAll((arg) => arg()).ToArray());
        }
    }

    public class BaseUI : VisualElement, IDisposable
    {
		// Manage properties
        public bool Enabled = true;
		public int LineSpace = 0;
		public Vector2 BorderSpace = new Vector2(10, 5);
		public Vector2 TextBound;

        SpriteBatch _spriteBatch;
        Texture2D _backgroundTexture;
        SpriteFont _font;
		List<TextDrawInfo> _processText;

		protected Vector2 Position = Vector2.Zero;
		protected Vector2 Size = new Vector2(200, 100);
		protected bool AutoSizeX = true;
		protected bool AutoSizeY = true;

        protected string TextureName;
        protected string FontName;

        List<TextElement> _textElements = new List<TextElement>();

        public BaseUI(Destiny game)
            : base(game)
        {
        }

		public void Switch()
		{ Enabled = !Enabled; }

		public void AddText(string text, params Func<object>[] arguments)
        {
            _textElements.Add(new TextElement() { Text = text, Arguments = arguments.ToList() });
        }

        public void ClearText()
        {
            _textElements.Clear();
        }

		override public void LoadContent()
		{
			base.LoadContent();
			_spriteBatch = new SpriteBatch(Device);
            if (FontName != null)
                _font = Content.Load<SpriteFont>(FontName);
            if (TextureName != null)
                _backgroundTexture = Content.Load<Texture2D>(TextureName);
        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (Enabled)
			{
				ProcessText();
				if (AutoSizeX || AutoSizeY)
				{
					if (AutoSizeX)
						Size.X = TextBound.X;
					if (AutoSizeY)
						Size.Y = TextBound.Y;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			if (Enabled)
			{
				_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
				if (_backgroundTexture != null)
				{
					DrawBackground(_spriteBatch);
				}
				if (_font != null && _processText != null)
					DrawText(_spriteBatch);
				_spriteBatch.End();
			}
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            int x = 0;
            int width = _backgroundTexture.Width;
            bool flipX = false;

            while (x < Size.X)
            {
                if ((x + width) > Size.X) 
					width = (int)Size.X - x;

                int y = 0;
                int height = _backgroundTexture.Height;
                bool flipY = false;

                while (y < Size.Y)
                {
                    if ((y + height) > Size.Y) 
						height = (int)Size.Y - y;
					var destRect = new Rectangle((int)(x + Position.X), (int)(y + Position.Y), width, height);
					var srcRect = new Rectangle(flipX ? (_backgroundTexture.Width - width) : 0, flipY ? (_backgroundTexture.Height - height) : 0, width, height);
					var effects = (flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipY ? SpriteEffects.FlipVertically : SpriteEffects.None);
					spriteBatch.Draw(_backgroundTexture, destRect, srcRect, Color.White, 0, Vector2.Zero, effects, 0);
                    y += height;
                    flipY = !flipY;
                }
                x += width;
                flipX = !flipX;
            }
        }

		struct TextDrawInfo
		{
			public string FormatedText;
			public Vector2 TextSize;
			public Vector2 Position;

		};

		private void ProcessText()
		{
			Vector2 offset = BorderSpace;
			TextBound = Vector2.Zero;
			_processText = new List<TextDrawInfo>();
			foreach (var element in _textElements)
			{
				string s = element.GetFormatedText();
				var textMeasure = _font.MeasureString(s);
				Vector2 textSize = new Vector2((int)textMeasure.X, (int)textMeasure.Y);
				_processText.Add(new TextDrawInfo()
				{
					FormatedText = s,
					TextSize = textSize,
					Position = offset,
				});
				offset += new Vector2(0, textSize.Y + LineSpace);
				TextBound.X = offset.X + Math.Max(textMeasure.X, TextBound.X);
				TextBound.Y = offset.Y;
			};
			TextBound += BorderSpace;
		}

		private void DrawText(SpriteBatch spriteBatch)
        {
            _processText.ForEach((element) =>
                {
					spriteBatch.DrawString(_font, element.FormatedText, Position + element.Position, Color.White);
                });
        }


		public override void UnloadContent()
		{
			base.UnloadContent();
			if (_spriteBatch != null) _spriteBatch.Dispose();
			if (_backgroundTexture != null) _backgroundTexture.Dispose();
		}
    }
}

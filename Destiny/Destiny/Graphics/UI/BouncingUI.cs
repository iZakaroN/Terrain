using Destiny.Input;
using Destiny.Input.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Graphics.UI
{
    class BouncingUI : BaseUI, IClickActionElement 
    {
        Vector2 _spriteSpeed = Vector2.One * 200;

        public BouncingUI(Destiny game)
			: base(game)
		{
			game.Controller.Register(this);
			Enabled = false;
			AutoSizeX = false;
			AutoSizeY = false;
			TextureName = @"Texture";
            FontName = @"Font1";
            Size = new Vector2(100, 100);
		}

        List<ClickAction> IActionElement<ClickEvent, ClickAction>.Actions
        {
            get
            {
                return new List<ClickAction>()
					{
						new ClickAction(KeyboardController.GetEvent(Keys.F3), Switch), 
					};
            }
        }

        public void Switch(GameTime gt, ClickEvent e)
        { Enabled = !Enabled; }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			// Move the sprite by speed, scaled by elapsed time.
            var position = Position;
            position += _spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            int MaxX = Device.Viewport.Width - (int)Size.X;
            int MinX = 0;
            int MaxY = Device.Viewport.Height - (int)Size.Y;
            int MinY = 0;

            // Check for bounce.
            if (position.X > MaxX)
            {
                _spriteSpeed.X *= -1;
                position.X = MaxX;
            }
            else if (position.X < MinX)
            {
                _spriteSpeed.X *= -1;
                position.X = MinX;
            }

            if (position.Y > MaxY)
            {
                _spriteSpeed.Y *= -1;
                position.Y = MaxY;
            }
            else if (Position.Y < MinY)
            {
                _spriteSpeed.Y *= -1;
                position.Y = MinY;
            }
            Position = position;
        }

    }
}

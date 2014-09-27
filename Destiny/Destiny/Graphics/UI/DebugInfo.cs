using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Destiny.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Destiny.Input.Actions;

namespace Destiny.Graphics.UI
{
	public class DebugInfo : BaseUI, IClickActionElement 
	{
		public DebugInfo(Destiny game)
			: base(game)
		{
			game.Controller.Register(this);
			Enabled = false;
            TextureName = @"Texture";
            FontName = @"Font1";
            Position = new Vector2(0, 40);
            Size = new Vector2(200, 500);
            AddText("FPS: {0}", GetFPS);
            AddText("Solid: {0}", GetSolidEnabled);
            AddText("Culling: {0}", GetCullEnabled);
        }

        TimeSpan ElapsedGameTime = TimeSpan.FromSeconds(1);

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			ElapsedGameTime = gameTime.ElapsedGameTime;
		}

		private object GetFPS()
		{
			return TimeSpan.FromSeconds(1).Ticks/ElapsedGameTime.Ticks;
		}

        private object GetCullEnabled()
        {
            return Game.CullEnabled;
        }

        private object GetSolidEnabled()
        {
            return Game.SolidEnabled;
        }

        List<ClickAction> IActionElement<ClickEvent, ClickAction>.Actions
		{
			get
			{
                return new List<ClickAction>()
					{
						new ClickAction(KeyboardController.GetEvent(Keys.F2), Switch), 
					};
			}
		}

		public void Switch(GameTime gt, ClickEvent e)
		{ Enabled = !Enabled; }

	}
}

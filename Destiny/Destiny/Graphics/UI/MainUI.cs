using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Destiny.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Destiny.Input.Actions;

namespace Destiny.Graphics.UI
{
    public class MainUI : BaseUI, IClickActionElement 
	{
		public MainUI(Destiny game)
			: base(game)
		{
			game.Controller.Register(this);
			Enabled = true;
			AutoSizeX = false;
            TextureName = @"Texture";
            FontName = @"Font1";
			AddText("Keys:");
        }

		override public void LoadContent()
		{
			base.LoadContent();
			Size.X = Device.Viewport.Width;
		} 

        List<ClickAction> IActionElement<ClickEvent, ClickAction>.Actions
        {
            get
            {
                return new List<ClickAction>()
					{
						new ClickAction(KeyboardController.GetEvent(Keys.Escape), Exit), 
						new ClickAction(KeyboardController.GetEvent(Keys.F1), Switch), 
					};
            }
        }

        public void Switch(GameTime gt, ClickEvent e)
		{ Enabled = !Enabled; }

        public void Exit(GameTime gt, ClickEvent e)
		{ Game.Exit(); }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Destiny.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Destiny.Graphics.UI
{
	public class MainUI : BaseUI, IPressedActionElement 
	{
		public MainUI(Destiny game)
			: base(game, @"Font1", @"Texture")
		{
			game.Controller.Register(this);
			Enabled = false;
		}

		List<PressedAction> IActionElement<PressedEvent, PressedAction>.Actions
		{
			get
			{
				return new List<PressedAction>()
					{
						new PressedAction(KeyboardController.GetEvent(Keys.Escape), Exit), 
						new PressedAction(KeyboardController.GetEvent(Keys.F2), Switch), 
					};
			}
		}

		public void Switch(GameTime gt, PressedEvent e)
		{ Enabled = !Enabled; }

		public void Exit(GameTime gt, PressedEvent e)
		{ Game.Exit(); }

	}
}

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
    public class MainUI : BaseUI
	{
		public BouncingUI BouncingUI;
		public DebugInfo DebugUI;

		public MainUI(Destiny game)
			: base(game)
		{
			AddChild(BouncingUI = new BouncingUI(game));
			AddChild(DebugUI = new DebugInfo(game));

			Enabled = true;
			AutoSizeX = false;
            TextureName = @"Texture";
            FontName = @"Font1";
			AddText("Keys:");
        }

		protected override void LoadSelf()
		{
			base.LoadSelf();
			Size.X = Device.Viewport.Width;
		}

        public void Exit()
		{ Game.Exit(); }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Destiny.Graphics.UI;

namespace Destiny.Graphics.World
{
	public class World : VisualElement, IDisposable
	{
		public Terrain Terrain;
		public MainUI UI;
		public DebugInfo DebugUI;
		
		public Avatar Avatar;

		public World(Destiny game) : base(game)
		{

			Terrain = new TerrainCube(game);
			UI = new MainUI(game);
			DebugUI = new DebugInfo(game);
			Avatar = new Avatar(game);

			Childs.Add(Terrain);
			Childs.Add(UI);
			Childs.Add(DebugUI);
			Childs.Add(Avatar);
			Childs.Add(new BouncingUI(game));
		}
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UI.Dispose();
                DebugUI.Dispose();
            }
        }
        #endregion Dispose
    }
}

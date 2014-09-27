using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Destiny
{
    abstract public class VisualElement
    {
        public Destiny Game;

		readonly public List<VisualElement> Childs = new List<VisualElement>();

        public VisualElement()
        {
        }

        public VisualElement(Destiny game)
        {
            Game = game;
        }


        public GraphicsDevice Device { get { return Game.GraphicsDevice; } }
        public ContentManager Content { get { return Game.Content; } }
		public Effect Effect { get { return Game.Effect; } }

		public virtual void LoadContent()
        {
			Childs.ForEach(vo => vo.LoadContent());
		}

		public virtual void UnloadContent()
        {
			Childs.ForEach(vo => vo.UnloadContent());
		}

		public virtual void Update(GameTime gameTime)
        {
			Childs.ForEach(vo => vo.Update(gameTime));
		}

		public virtual void Draw(GameTime gameTime)
        {
            Childs.ForEach(vo => vo.Draw(gameTime));
        }
    }
}

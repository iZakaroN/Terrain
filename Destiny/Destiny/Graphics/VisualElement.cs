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
            Initialize(game);
        }

        public virtual void Initialize(Destiny game)
        {
            Game = game;
        }


        public GraphicsDevice Device { get { return Game.GraphicsDevice; } }
        public ContentManager Content { get { return Game.Content; } }
		public Effect Effect { get { return Game.Effect; } }

        virtual public void LoadContent()
        {
			Childs.ForEach(vo => vo.LoadContent());
		}

        virtual public void UnloadContent()
        {
			Childs.ForEach(vo => vo.UnloadContent());
		}

        virtual public void Update(GameTime gameTime)
        {
			Childs.ForEach(vo => vo.Update(gameTime));
		}

        virtual public void Draw(GameTime gameTime)
        {
            Childs.ForEach(vo => vo.Draw(gameTime));
        }

        /*virtual public void Draw()
		{
			Childs.ForEach(vo => vo.Draw());
		}*/


    }
}

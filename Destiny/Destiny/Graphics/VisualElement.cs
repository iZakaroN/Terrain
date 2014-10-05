using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Destiny
{
	public interface IVisualElement
	{
		void LoadContent();
		void UnloadContent();
		void Update(GameTime gameTime);
		void Draw(GameTime gameTime);
		bool Visible { get; set; }
		bool Active { get; set; }
		int Count { get; }
	}

	abstract public class VisualElement : IDisposable, IVisualElement
	{
		public Destiny Game;
		bool _visible = true;
		bool _active = true;
		bool ContentLoaded;

		readonly public List<VisualElement> Childs = new List<VisualElement>();

		public VisualElement(Destiny game)
		{
			Game = game;
		}


		public GraphicsDevice Device { get { return Game.GraphicsDevice; } }
		public ContentManager Content { get { return Game.Content; } }
		public Effect Effect { get { return Game.Effect; } }

		#region IVisualElement

		public int Count { get { return Childs.Count(); } }
		public bool Visible { get { return _visible; } set { _visible = value; } }
		public bool Active { get { return _active; } set { _active = value; } }

		public virtual void LoadContent()
		{
			Childs.ForEach(vo => vo.LoadContent());
		}

		public virtual void UnloadContent()
		{
			Childs.ForEach(vo => vo.UnloadContent());
		}

		public void Update(GameTime gameTime)
		{
			if (Active)
			{
				Childs.ForEach(vo => vo.Update(gameTime));
				UpdateSelf(gameTime);
			}
		}

		public void Draw(GameTime gameTime)
		{
			if (Visible)
			{
				DrawSelf(gameTime);
			}
		}

		#endregion IVisualElement

		#region Virtaul
		protected virtual void UpdateSelf(GameTime gameTime)
		{
		}

		protected virtual void DrawSelf(GameTime gameTime)
		{
			Childs.ForEach(vo => vo.Draw(gameTime));
		}
		#endregion Virtaul

		#region Dispose
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnloadContent();
			}
		}
		#endregion Dispose
	}
}

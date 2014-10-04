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

	public abstract class VisualElement : IDisposable, IVisualElement
    {
        public Destiny Game;

		bool _visible = true;
		bool _active = true;
		bool ContentLoaded;
		readonly protected List<IVisualElement> Childs = new List<IVisualElement>();

        public VisualElement(Destiny game)
        {
            Game = game;
        }

        protected GraphicsDevice Device { get { return Game.GraphicsDevice; } }
		protected ContentManager Content { get { return Game.Content; } }
		protected Effect Effect { get { return Game.Effect; } }

		#region IVisualElement

		public int Count { get { return Childs.Count(); } }

		public bool Visible { get { return _visible; } set { _visible = value; } }
		public bool Active { get { return _active; } set { _active = value; } }

		public void LoadContent()
        {
			Childs.ForEach(vo => vo.LoadContent());
			ContentLoaded = true;
			LoadSelf();
		}

		public void UnloadContent()
        {
			Childs.ForEach(vo => vo.UnloadContent());
			ContentLoaded = false;
			UnloadSelf();
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
				DrawChilds(() => DrawChilds(gameTime));
				DrawSelf();
			}
        }
		#endregion IVisualElement

		#region Virtaul
		protected virtual void LoadSelf()
		{
		}

		protected virtual void UnloadSelf()
		{
		}

		protected virtual void UpdateSelf(GameTime gameTime)
		{
		}

		protected virtual void DrawSelf()
		{
		}

		protected virtual void DrawChilds(Action drawChilds)
		{
			drawChilds();
		}
		#endregion Virtaul

		#region Private
		private void DrawChilds(GameTime gameTime)
		{
			Childs.ForEach(vo => vo.Draw(gameTime));
		}
		#endregion Private

		#region Protected
		protected void AddChild(IVisualElement child)
		{
			if (ContentLoaded)
				child.LoadContent();
			Childs.Add(child);
		}

		protected void RemoveChild(IVisualElement child)
		{
			Childs.Remove(child);
			if (ContentLoaded)
				child.UnloadContent();
		}

		protected void RemoveAllChilds()
		{
			if (ContentLoaded)
				Childs.ForEach((ve) => ve.UnloadContent());
			Childs.Clear();
		}
		#endregion Protected

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

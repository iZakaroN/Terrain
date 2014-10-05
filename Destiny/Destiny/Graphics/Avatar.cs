using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Destiny.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Destiny.Graphics;
using Destiny.Input.Actions;

namespace Destiny
{
	public class Avatar : VisualElement
	{
		public float AngleSpeed = 0.0020f;
		public float MouseSpeed = 1f;
		public float MoveSpeed = 0.053f;

		public Avatar(Destiny game)
			: base(game)
		{
			AddChild(new Crosshair(game));
		}

		#region Movement
		public void MoveLeft(GameTime gameTime, PressedEvent evnt)
		{
			Position -= Side * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void MoveRight(GameTime gameTime, PressedEvent evnt)
		{
			Position += Side * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void MoveForward(GameTime gameTime, PressedEvent evnt)
		{
			Position += Forward * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void MoveBackward(GameTime gameTime, PressedEvent evnt)
		{
			Position -= Forward * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void MoveDown(GameTime gameTime, PressedEvent evnt)
		{
			Position -= LookUp * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void MoveUp(GameTime gameTime, PressedEvent evnt)
		{
			Position += LookUp * gameTime.ElapsedGameTime.Milliseconds * MoveSpeed;
		}

		public void RotateLeft(GameTime gameTime, PressedEvent evnt)
		{
			AngleH += gameTime.ElapsedGameTime.Milliseconds * AngleSpeed;
		}

		public void RotateRight(GameTime gameTime, PressedEvent evnt)
		{
			AngleH -= gameTime.ElapsedGameTime.Milliseconds * AngleSpeed;
		}

		public void RotateUp(GameTime gameTime, PressedEvent evnt)
		{
			AngleV += gameTime.ElapsedGameTime.Milliseconds * AngleSpeed;
		}

		public void RotateDown(GameTime gameTime, PressedEvent evnt)
		{
			AngleV -= gameTime.ElapsedGameTime.Milliseconds * AngleSpeed;
		}

		public void RotateH(GameTime gameTime, MotionEvent evnt)
		{
			AngleH += evnt.Motion * AngleSpeed * MouseSpeed;
		}

		public void RotateV(GameTime gameTime, MotionEvent evnt)
		{
			AngleV -= evnt.Motion * AngleSpeed * MouseSpeed;
		}

		#endregion Movement

		#region VisualElement
		protected override void UpdateSelf(GameTime gameTime)
		{
			base.UpdateSelf(gameTime);
			SetPointing();
		}
		#endregion VisualElement

		#region Actions
		public void SetMapPoint()
		{
			Game.World.Terrain.SetMapPoint(GetPointingLocation());
		}
		#endregion

		#region Privite
		private void SetPointing()
		{
			Game.World.Terrain.SetPointing(GetPointingLocation());
		}

		private Vector3 GetPointingLocation()
		{
			return Position + Forward * 2.5f;
		}
		#endregion Privite

		#region Camera
		public Vector3 Position
		{
			get { return Game.Camera.Position; }
			set { Game.Camera.Position = value; }
		}

		public float AngleH
		{
			get { return Game.Camera.AngleH; }
			set { Game.Camera.AngleH = value; }
		}

		public float AngleV
		{
			get { return Game.Camera.AngleV; }
			set { Game.Camera.AngleV = value; }
		}

		public Vector3 Side
		{
			get { return Game.Camera.Side; }
		}

		public Vector3 Forward
		{
			get { return Game.Camera.Forward; }
		}

		public Vector3 LookUp
		{
			get { return Game.Camera.LookUp; }
		}

		#endregion Camera

	}
}

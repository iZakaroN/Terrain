using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Destiny.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Destiny.Graphics;

namespace Destiny
{
	public class Avatar : VisualElement, IPressedActionElement, IMotionActionElement
	{
		public float AngleSpeed = 0.0020f;
		public float MouseSpeed = 1f;
		public float MoveSpeed = 0.053f;

		public Avatar(Destiny game)
			: base(game)
		{
			game.Controller.Register(this);
			Childs.Add(new Crosshair(game));
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
			AngleH -= evnt.Motion * AngleSpeed * MouseSpeed;
		}

		public void RotateV(GameTime gameTime, MotionEvent evnt)
		{
			AngleV -= evnt.Motion * AngleSpeed * MouseSpeed;
		}

		#endregion Movement

		#region Actions
		public void SetCube(GameTime gameTime, PressedEvent evnt)
		{
			Game.World.Terrain.AddCube(RoundCubeCoordinated(Position+Forward*1.9f));
		}

		public Vector3 RoundCubeCoordinated(Vector3 v)
		{
			return new Vector3((int)v.X, (int)v.Y, (int)v.Z);
		}


		#endregion

		#region Controller
		List<PressedAction> IActionElement<PressedEvent, PressedAction>.Actions
		{
			get
			{
				return new List<PressedAction>()
                {

                    new PressedAction(KeyboardController.GetEvent(Keys.Left), RotateLeft), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Right), RotateRight), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Up), RotateUp), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Down), RotateDown), 

                    new PressedAction(KeyboardController.GetEvent(Keys.W), MoveForward), 
                    new PressedAction(KeyboardController.GetEvent(Keys.S), MoveBackward), 
                    new PressedAction(KeyboardController.GetEvent(Keys.A), MoveLeft), 
                    new PressedAction(KeyboardController.GetEvent(Keys.D), MoveRight), 

                    new PressedAction(KeyboardController.GetEvent(Keys.Space), MoveUp), 
                    new PressedAction(KeyboardController.GetEvent(Keys.LeftControl), MoveDown), 
                    new PressedAction(MouseController.GetEvent(MouseEvents.RightButton), SetCube), 
                };
			}
		}

		List<MotionAction> IActionElement<MotionEvent, MotionAction>.Actions
		{
			get
			{
				return new List<MotionAction>()
                {
                    new MotionAction(MouseController.GetEvent(MouseEvents.X), RotateH), 
                    new MotionAction(MouseController.GetEvent(MouseEvents.Y), RotateV), 
                };

			}
		}
		#endregion Controller

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

        public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}

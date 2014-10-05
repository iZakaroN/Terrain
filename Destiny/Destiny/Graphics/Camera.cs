using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Destiny.Input;

namespace Destiny
{
	public class Camera : VisualElement
	{

		public Vector3 Position = new Vector3(0, 200, -300);

		/// <summary>
		/// Yaw
		/// </summary>
		public float AngleV = 0;
		/// <summary>
		/// pitch
		/// </summary>
		public float AngleH = MathHelper.Pi / 6;

		public Vector3 Forward { get; private set; }
		public Vector3 Side { get; private set; }
		public Vector3 LookUp { get; private set; }
		public Quaternion Rotation { get; private set; }

		Matrix viewMatrix;
		Matrix projectionMatrix;

		Vector3 VectorH = new Vector3(-1, 0, 0);
		Vector3 VectorV = new Vector3(0, 1, 0);
		Vector3 VectorF = new Vector3(0, 0, 1);



		public Camera(Destiny game)
			: base(game)
		{
		}

		override public void LoadContent()
		{
			base.LoadContent();
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3, Device.Viewport.AspectRatio, 0.1f, 1500.0f);
			CalculateView();
		}


		void CalculateView()
		{
			Rotation = Quaternion.CreateFromYawPitchRoll(AngleV, AngleH, 0);

			Forward = Vector3.Transform(VectorF, Rotation); Forward.Normalize();
			Side = Vector3.Transform(VectorH, Rotation); Side.Normalize();
			LookUp = Vector3.Transform(VectorV, Rotation); LookUp.Normalize();

			viewMatrix = Matrix.CreateLookAt(Position, Position + Forward, LookUp);
		}

		protected override void DrawSelf(GameTime gameTime)
		{
			base.DrawSelf(gameTime);
			CalculateView();
			Effect.Parameters["xView"].SetValue(viewMatrix);
			Effect.Parameters["xProjection"].SetValue(projectionMatrix);

			Matrix worldMatrix = Matrix.Identity;// CreateWorld(new Vector3(0, 0, 0), VectorF, VectorV);//Identity;//.CreateRotationY(3 * _angle);
			Effect.Parameters["xWorld"].SetValue(worldMatrix);
		}

	}
}
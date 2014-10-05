using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Destiny.Input;
using Destiny.Graphics.World;
using Destiny.Input.Actions;
using System.Diagnostics;

namespace Destiny
{
	class MyGraphicsDeviceManager : GraphicsDeviceManager
	{
		public MyGraphicsDeviceManager(Game game)
			: base(game)
		{
		}

		protected override GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice)
		{
			return base.FindBestDevice(anySuitableDevice);
		}

		protected override void RankDevices(List<GraphicsDeviceInformation> foundDevices)
		{
			foundDevices.RemoveAll((gdi) => gdi.Adapter.Description!="ATI Radeon HD 4200");
		}
	}
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Destiny : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		GraphicsDevice Device { get { return graphics.GraphicsDevice; } }


		public Effect Effect { get; private set; }

		public Camera Camera { get; private set; }

		public Controller Controller { get; private set; }

		public World World { get; private set; }

		InputControlMapper InputControlMapper;

		List<VisualElement> _visualObjects = new List<VisualElement>();


		public Destiny()
		{
			var adapters = GraphicsAdapter.Adapters;
			foreach (var adapter in adapters)
				Debug.WriteLine(string.Format("Display adapter: {0}", adapter.DeviceName));
			graphics = new MyGraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			this.graphics.PreferredBackBufferWidth = 1440;
			this.graphics.PreferredBackBufferHeight = 900;
			this.graphics.IsFullScreen = true;

			Device.SamplerStates[0] = new SamplerState() { Filter = TextureFilter.Linear, MipMapLevelOfDetailBias = 0, MaxAnisotropy = 4, MaxMipLevel = 0 };
			graphics.ApplyChanges();
			Camera = new Camera(this);
			World = new World(this);

			InputControlMapper = new Input.InputControlMapper(this, World.Avatar, World.UI);
			Controller = new Input.Controller(this);
			Controller.Register(InputControlMapper);

			_visualObjects.Add(Camera);
			_visualObjects.Add(World);
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			Effect = Content.Load<Effect>("effects");
			_visualObjects.ForEach(vo => vo.LoadContent());

		}
		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			_visualObjects.ForEach(vo => vo.UnloadContent());
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			Controller.Update(gameTime);

			_visualObjects.ForEach(vo => vo.Update(gameTime));
			base.Update(gameTime);
		}

		public bool SolidEnabled = true;
		public bool CullEnabled = true;
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Black);

			//			Effect.CurrentTechnique = Effect.Techniques["ColoredNoShading"];
			//Camera.Draw(gameTime);
			SetupLightning();

			_visualObjects.ForEach(vo => vo.Draw(gameTime));

			base.Draw(gameTime);
		}

		private void SetupLightning()
		{
			Effect.Parameters["xEnableLighting"].SetValue(true);
			//Effect.Parameters["sampler"].SetValue( = SamplerState.PointClamp;
			Vector3 lightDirection = new Vector3(-10f, -10f, -10f);
			lightDirection.Normalize();
			Effect.Parameters["xLightDirection"].SetValue(lightDirection);
			Effect.Parameters["xAmbient"].SetValue(0.5f);
		}

		public void SwitchSolid()
		{ SolidEnabled = !SolidEnabled; }

		public void SwitchCull()
		{ CullEnabled = !CullEnabled; }

	}
}

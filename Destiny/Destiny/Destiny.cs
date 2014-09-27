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

namespace Destiny
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Destiny : Microsoft.Xna.Framework.Game, IClickActionElement
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice Device { get { return graphics.GraphicsDevice; } }


        public Effect Effect { get; private set; }

        public Camera Camera { get; private set; }

        public Controller Controller { get; private set; }

        public World World { get; private set; }

        List<VisualElement> _visualObjects = new List<VisualElement>();


        public Destiny()
        {
            graphics = new GraphicsDeviceManager(this);
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
            this.graphics.PreferredBackBufferWidth = 1920;
            this.graphics.PreferredBackBufferHeight = 1080;

            this.graphics.IsFullScreen = false;

            Device.SamplerStates[0] = new SamplerState() { Filter = TextureFilter.PointMipLinear, MipMapLevelOfDetailBias = 2, MaxAnisotropy = 4, MaxMipLevel = 8 };
            graphics.ApplyChanges();
            Controller = new Input.Controller(this);
            Controller.Register(this);
            Camera = new Camera(this);
            World = new World(this);
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

            Effect.CurrentTechnique = Effect.Techniques["Textured"];
            //			Effect.CurrentTechnique = Effect.Techniques["ColoredNoShading"];
            //Camera.Draw(gameTime);
            Effect.Parameters["xEnableLighting"].SetValue(true);
            //Effect.Parameters["sampler"].SetValue( = SamplerState.PointClamp;
            Vector3 lightDirection = new Vector3(-10f, -10f, -10f);
            lightDirection.Normalize();
            Effect.Parameters["xLightDirection"].SetValue(lightDirection);
            Effect.Parameters["xAmbient"].SetValue(0.5f);
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullEnabled ? CullMode.CullCounterClockwiseFace : CullMode.None;
            rs.FillMode = SolidEnabled ? FillMode.Solid : FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;

            _visualObjects.ForEach(vo => vo.Draw(gameTime));



            base.Draw(gameTime);
        }


        List<ClickAction> IActionElement<ClickEvent, ClickAction>.Actions
        {
            get
            {
                return new List<ClickAction>()
					{
						new ClickAction(KeyboardController.GetEvent(Keys.F5), SwitchSolid), 
						new ClickAction(KeyboardController.GetEvent(Keys.F6), SwitchCull), 
					};
            }
        }

        public void SwitchSolid(GameTime gt, ClickEvent e)
        { SolidEnabled = !SolidEnabled; }

        public void SwitchCull(GameTime gt, ClickEvent e)
        { CullEnabled = !CullEnabled; }

    }
}

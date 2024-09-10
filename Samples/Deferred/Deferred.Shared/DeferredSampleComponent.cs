using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Samples.Deferred
{
    internal class DeferredSampleComponent : DrawableGameComponent
    {
        ContentManager Content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState previousKeyboardState;
        
        bool useLightA = true;
        bool useLightB = true;
        bool useLightC = true; 
        bool rotate = true;

        Vector3 cameraPosition;

        Matrix world;
        Matrix projection;
        Matrix view;

        Spaceship spaceship;
        Vector3 spaceshipPos = Vector3.Zero;
        float time = 0;

        DeferredRendering _deferredRendering;

        const float LightAIntensity = 10f;
        const float LightBIntensity = 1f;
        const float LightCIntensity = 3f;

        float lightAcurrentIntensity = LightAIntensity;
        float lightBcurrentIntensity = LightBIntensity;
        float lightCcurrentIntensity = LightCIntensity;

        public DeferredSampleComponent(Game game) : base(game)
        {
        }

        /// <summary>Initializes the component. Used to load non-graphical resources.</summary>
        public override void Initialize()
        {
            Content = new ContentManager(Game.Services, "Content");

            base.Initialize();
        }

        /// <summary>Load graphical resources needed by this component.</summary>
        protected override void LoadContent()
        {
            // Create and load our tank
            spaceship = new Spaceship();
            spaceship.Load(Content);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 2000f, 6000f);

            _deferredRendering = new DeferredRendering(GraphicsDevice, Content);
        }

        /// <summary>Unload graphical resources needed by this component.</summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>Update the component.</summary>
        /// <param name="gameTime">GameTime of the Game.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.D1) && !previousKeyboardState.IsKeyDown(Keys.D1))
                useLightA = !useLightA;
            if (keyState.IsKeyDown(Keys.D2) && !previousKeyboardState.IsKeyDown(Keys.D2))
                useLightB = !useLightB;
            if (keyState.IsKeyDown(Keys.D3) && !previousKeyboardState.IsKeyDown(Keys.D3))
                useLightC = !useLightC;
            if (keyState.IsKeyDown(Keys.D4) && !previousKeyboardState.IsKeyDown(Keys.D4))
                rotate = !rotate;

            if (rotate)
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float lightAtargetIntensity = (useLightA) ? LightAIntensity : 0f;
            float lightBtargetIntensity = (useLightB) ? LightBIntensity : 0f;
            float lightCtargetIntensity = (useLightC) ? LightCIntensity : 0f;
            lightAcurrentIntensity += (lightAtargetIntensity - lightAcurrentIntensity) * (0.1f);
            lightBcurrentIntensity += (lightBtargetIntensity - lightBcurrentIntensity) * (0.1f);
            lightCcurrentIntensity += (lightCtargetIntensity - lightCcurrentIntensity) * (0.1f);

            world = Matrix.CreateFromAxisAngle(Vector3.Up, time);
            cameraPosition = new Vector3(0, 2800f, 2800f);
            
            view = Matrix.CreateLookAt(
                   cameraPosition,
                   Vector3.Zero,
                   Vector3.Up);

            previousKeyboardState = keyState;
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            _deferredRendering.SetGBuffer();
            _deferredRendering.ClearGBuffer();
            
            //draw models
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spaceship.DrawDeferred(GraphicsDevice, _deferredRendering.basicEffect, world, view, projection);

            _deferredRendering.ResolveGBuffer();
            GraphicsDevice.SetRenderTarget(_deferredRendering.lightRT);
            GraphicsDevice.Clear(Color.Transparent);

            // Draw lights

            //float ambient = 0.1f;
            //GraphicsDevice.Clear(new Color(ambient, ambient, ambient, 0f));

            var lightPos = spaceshipPos + new Vector3(2000,100,0);
            _deferredRendering.DrawPointLight(
                lightPos, Color.Goldenrod, 2000f, lightAcurrentIntensity,
                view, projection, cameraPosition);

            var light2Pos = spaceshipPos + new Vector3(0, 800, 1000);
            _deferredRendering.DrawPointLight(
                light2Pos, Color.CornflowerBlue, 900f, lightBcurrentIntensity,
                view, projection, cameraPosition);

            var spotLightPos = spaceshipPos + new Vector3(-1000,1000,0);
            var lightDirection = spaceshipPos - spotLightPos;
            _deferredRendering.DrawSpotLight(
                spotLightPos, Color.White, 1000f, lightCcurrentIntensity, lightDirection,
                MathHelper.ToRadians(3f), MathHelper.ToRadians(3f),
                view, projection, cameraPosition);
            
            _deferredRendering.Combine();

            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            _deferredRendering.DrawRTs(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("[F] PointLight A - ({0})", useLightA ? "ON" : "OFF"), new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(font, String.Format("[2] PointLight B - ({0})", useLightB ? "ON" : "OFF"), new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(font, String.Format("[3] SpotLight C - ({0})", useLightC ? "ON" : "OFF"), new Vector2(20, 60), Color.White);
            spriteBatch.DrawString(font, String.Format("[4] Rotate - ({0})", rotate ? "ON" : "OFF"), new Vector2(20, 80), Color.White);
            spriteBatch.End();
        }

    }
}

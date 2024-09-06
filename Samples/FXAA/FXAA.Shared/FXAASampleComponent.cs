using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Samples.FXAA
{
    internal class FXAASampleComponent : DrawableGameComponent
    {
        ContentManager Content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState previousKeyboardState;

        bool useFXAA = true;
        bool rotate = true;
        
        Vector3 cameraPosition;

        Matrix world;
        Matrix projection;
        Matrix view;

        Spaceship spaceship;
        Vector3 spaceshipPos = Vector3.Zero;
        float time = 0;
        
        AntiAliasing _antiAliasing;


        public FXAASampleComponent(Game game) : base(game)
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
                MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 10f, 10000f);
            
            _antiAliasing = new AntiAliasing(GraphicsDevice);
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


            if (keyState.IsKeyDown(Keys.F1) && !previousKeyboardState.IsKeyDown(Keys.F1))
                useFXAA = !useFXAA;
            if (keyState.IsKeyDown(Keys.F2) && !previousKeyboardState.IsKeyDown(Keys.F2))
                rotate = !rotate;

            if (rotate)
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _antiAliasing.SetRenderTarget(GraphicsDevice.Viewport, Color.Black);
            spaceship.Draw(world, view, projection);
            GraphicsDevice.SetRenderTarget(null);

            _antiAliasing.DrawRenderTarget( (useFXAA ? 3 : 0), GraphicsDevice.Viewport, false);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("[F1] FXAA - ({0})", useFXAA ? "ON" : "OFF"), new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(font, String.Format("[F2] Rotate - ({0})", rotate ? "ON" : "OFF"), new Vector2(20, 40), Color.White);
            spriteBatch.End();
        }

    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Samples.Animation
{
    public class AnimationGame : Game
    {
        GraphicsDeviceManager graphics;

        AnimationSampleComponent _animationSampleComponent;

        public AnimationGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _animationSampleComponent = new AnimationSampleComponent(this);
            Components.Add(_animationSampleComponent);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);
        }

    }
}

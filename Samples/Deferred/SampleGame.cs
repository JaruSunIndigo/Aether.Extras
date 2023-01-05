using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Samples.Deferred
{
    public class SampleGame : Game
    {
        GraphicsDeviceManager graphics;

        DeferredSampleComponent _deferredSampleComponent;

        public SampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            _deferredSampleComponent = new DeferredSampleComponent(this);
            Components.Add(_deferredSampleComponent);
        }

        protected override void LoadContent()
        {
        }
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                Exit();

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

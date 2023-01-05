using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Samples.SLMC
{
    internal class SLMCSampleComponent : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        ContentManager Content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState previousKeyboardState;
        
        int mipLevel = 0;
        Rectangle rtSize;
        RenderTarget2D rt;

        Texture2D tx;
        
        public SLMCSampleComponent(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            this.graphics = graphics;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            
            tx = Content.Load<Texture2D>("b_c0123");
            rtSize = new Rectangle(0, 0, tx.Width * (1+4), tx.Height);

            rt = new RenderTarget2D(GraphicsDevice, rtSize.Width, rtSize.Height);

            graphics.PreferredBackBufferWidth = (int)rtSize.Width;
            graphics.PreferredBackBufferHeight = (int)rtSize.Height;
            graphics.ApplyChanges();
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


            if (keyState.IsKeyDown(Keys.OemPlus) && !previousKeyboardState.IsKeyDown(Keys.OemPlus) && mipLevel < tx.LevelCount-1)
                mipLevel++;
            if (keyState.IsKeyDown(Keys.Add) && !previousKeyboardState.IsKeyDown(Keys.Add) && mipLevel < tx.LevelCount-1)
                mipLevel++;
            if (keyState.IsKeyDown(Keys.OemMinus) && !previousKeyboardState.IsKeyDown(Keys.OemMinus) && mipLevel > 0)
                mipLevel--;
            if (keyState.IsKeyDown(Keys.Subtract) && !previousKeyboardState.IsKeyDown(Keys.Subtract) && mipLevel > 0)
                mipLevel--;
            
            previousKeyboardState = keyState;
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            int mipLevel2 = (int)Math.Pow(2, mipLevel);
            var mipSize = rtSize;
            mipSize.Width /= mipLevel2;
            mipSize.Height /= mipLevel2;

            GraphicsDevice.SetRenderTarget(rt);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            {
                var destRect = new Rectangle(0, 0, tx.Width, tx.Height);
                destRect.X /= mipLevel2;
                destRect.Y /= mipLevel2;
                destRect.Width /= mipLevel2;
                destRect.Height /= mipLevel2;
                
                // draw all channels
                destRect.X = (tx.Width * 0) / mipLevel2;
                spriteBatch.Draw(tx, destRect, Color.White);

                // draw each channels
                destRect.X = (tx.Width * 1) / mipLevel2;
                spriteBatch.Draw(tx, destRect, new Color(1f, 0f, 0f, 0f));
                destRect.X = (tx.Width * 2) / mipLevel2;
                spriteBatch.Draw(tx, destRect, new Color(0f, 1f, 0f, 0f));
                destRect.X = (tx.Width * 3) / mipLevel2;
                spriteBatch.Draw(tx, destRect, new Color(0f, 0f, 1f, 0f));
                destRect.X = (tx.Width * 4) / mipLevel2;
                spriteBatch.Draw(tx, destRect, new Color(0f, 0f, 0f, 1f)); // NOTE: alpha channel is not visible                
            }
            spriteBatch.End();

            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            spriteBatch.Draw(rt, rtSize, mipSize, Color.White);
            spriteBatch.End();


            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("[+/-] MipLevel - ({0})", mipLevel), new Vector2(11, 11), Color.Black);
            spriteBatch.DrawString(font, String.Format("[+/-] MipLevel - ({0})", mipLevel), new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }

    }
}

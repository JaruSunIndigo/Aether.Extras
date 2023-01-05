using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Graphics;

namespace Samples.Atlas
{
    internal class AtlasSampleComponent : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        ContentManager Content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        KeyboardState previousKeyboardState;

        int mipLevel = 4;
        bool showAtlas = false;
        bool useGenerateBitmap = true;
        bool useMipmapPerSprite = true;
        RenderTarget2D rt;

        Rectangle atlasSize = new Rectangle(0, 0, 1024, 512);
        
        TextureAtlas atlasMipmapPerSprite;
        TextureAtlas atlasMipmap;
        TextureAtlas atlasNoMipmap;

        public AtlasSampleComponent(Game game, GraphicsDeviceManager graphics) : base(game)
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
            rt = new RenderTarget2D(GraphicsDevice, atlasSize.Width, atlasSize.Height);

            // Load Atlas
            atlasMipmapPerSprite = Content.Load<TextureAtlas>("atlasMipmapPerSprite");
            atlasMipmap = Content.Load<TextureAtlas>("atlasMipmap");
            atlasNoMipmap = Content.Load<TextureAtlas>("atlasNoMipmap");
            
            graphics.PreferredBackBufferWidth = atlasSize.Width;
            graphics.PreferredBackBufferHeight = atlasSize.Height;
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

            if (keyState.IsKeyDown(Keys.F1) && !previousKeyboardState.IsKeyDown(Keys.F1))
                useMipmapPerSprite = !useMipmapPerSprite;
            if (keyState.IsKeyDown(Keys.F2) && !previousKeyboardState.IsKeyDown(Keys.F2))
                useGenerateBitmap = !useGenerateBitmap;
            if (keyState.IsKeyDown(Keys.F3) && !previousKeyboardState.IsKeyDown(Keys.F3))
                showAtlas = !showAtlas;
            if (keyState.IsKeyDown(Keys.OemPlus) && !previousKeyboardState.IsKeyDown(Keys.OemPlus) && mipLevel < 10)
                mipLevel++;
            if (keyState.IsKeyDown(Keys.Add) && !previousKeyboardState.IsKeyDown(Keys.Add) && mipLevel < 10)
                mipLevel++;
            if (keyState.IsKeyDown(Keys.OemMinus) && !previousKeyboardState.IsKeyDown(Keys.OemMinus) && mipLevel > 0)
                mipLevel--;
            if (keyState.IsKeyDown(Keys.Subtract) && !previousKeyboardState.IsKeyDown(Keys.Subtract) && mipLevel > 0)
                mipLevel--;

            previousKeyboardState = keyState;
        }

        private void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch, TextureAtlas atlas)
        {
            var sprite18 = atlas.Sprites["18"];
            spriteBatch.Draw(sprite18, new Vector2(128, 128), Color.White);

            var spriteMushroom_2 = atlas.Sprites["Mushroom_2"];
            spriteBatch.Draw(spriteMushroom_2, new Vector2(256 + 128, 128), Color.White);

            var sprite10 = atlas.Sprites["10"];
            spriteBatch.Draw(sprite10, new Vector2(512, 128), Color.White);
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            int mipLevel2 = (int)Math.Pow(2, mipLevel);
            var mipSize = atlasSize;
            mipSize.Width /= mipLevel2;
            mipSize.Height /= mipLevel2;

            GraphicsDevice.SetRenderTarget(rt);
            GraphicsDevice.Clear(Color.Black);

            var currentAtlas = (useGenerateBitmap) ? (useMipmapPerSprite ? atlasMipmapPerSprite : atlasMipmap) : atlasNoMipmap;

            if (showAtlas)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(currentAtlas.Texture, mipSize, Color.White);
                spriteBatch.End();
            }
            else
            {
                var scaleMtx = Matrix.CreateScale(1f / mipLevel2);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, scaleMtx);
                // Draw sprites from Atlas
                DrawSprites(gameTime, spriteBatch, currentAtlas);
                spriteBatch.End();
            }


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
            spriteBatch.Draw(rt, atlasSize, mipSize, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("[F1] MipmapPerSprite - ({0})", useMipmapPerSprite ? "ON" : "OFF"), new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(font, String.Format("[F2] GenerateMipmap - ({0})", useGenerateBitmap ? "ON" : "OFF"), new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(font, String.Format("[F3] {0}", showAtlas ? "Show Sprites" : "Show Atlas"), new Vector2(20, 60), Color.White);
            spriteBatch.DrawString(font, String.Format("[+/-] MipLevel - ({0})", mipLevel), new Vector2(20, 80), Color.White);
            spriteBatch.End();
        }

    }
}

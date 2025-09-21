using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Graphics;
using nkast.Aether.Shaders;

namespace Samples.Tilemaps
{
    internal class TilemapSampleComponent : DrawableGameComponent
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
        Rectangle atlasSize = new Rectangle(0, 0, 1024, 512);
        RenderTarget2D rt;

        Tilemap tilemapMipmapPerSprite;
        Tilemap tilemapMipmap;
        Tilemap tilemapNoMipmap;

        public TilemapSampleComponent(Game game, GraphicsDeviceManager graphics) : base(game)
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

            // Load tilemap
            tilemapMipmapPerSprite = Content.Load<Tilemap>("tilemapMipmapPerSprite");
            tilemapMipmap = Content.Load<Tilemap>("tilemapMipmap");
            tilemapNoMipmap = Content.Load<Tilemap>("tilemapNoMipmap");

#if DEBUG
            using (var fs = File.Create("tilemapNoMipmapAtlas.png"))
                tilemapNoMipmap.TextureAtlas.SaveAsPng(fs, tilemapNoMipmap.TextureAtlas.Width, tilemapNoMipmap.TextureAtlas.Height);
            using (var fs = File.Create("tilemapMipmapAtlas.png"))
                tilemapNoMipmap.TextureAtlas.SaveAsPng(fs, tilemapMipmap.TextureAtlas.Width, tilemapNoMipmap.TextureAtlas.Height);
#endif

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

            if (keyState.IsKeyDown(Keys.D1) && !previousKeyboardState.IsKeyDown(Keys.D1))
                useMipmapPerSprite = !useMipmapPerSprite;
            if (keyState.IsKeyDown(Keys.D2) && !previousKeyboardState.IsKeyDown(Keys.D2))
                useGenerateBitmap = !useGenerateBitmap;
            if (keyState.IsKeyDown(Keys.D3) && !previousKeyboardState.IsKeyDown(Keys.D3))
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

        private void DrawTilemap(GameTime gameTime, Tilemap tilemap, Rectangle mipSize)
        {
            // setup tilemapEffect
            var viewport = GraphicsDevice.Viewport;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.Identity;
#if XNA
            halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
#endif

            tilemap.Effect.Projection = halfPixelOffset * projection;
                        
            // Draw tilemap
            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, tilemap.Effect);
            spriteBatch.Draw(tilemap.TextureMap, mipSize, Color.White);
            spriteBatch.End();
        }

        /// <summary>Draw this component.</summary>
        /// <param name="gameTime">The time elapsed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GraphicsDevice.SetRenderTarget(rt);
            GraphicsDevice.Clear(Color.Black);

            var currentTilemap = (useGenerateBitmap) 
                ? (useMipmapPerSprite ? tilemapMipmapPerSprite : tilemapMipmap)
                : (tilemapNoMipmap);

            int mipLevel2 = (int)Math.Pow(2, mipLevel);
            var mipSize = atlasSize;
            mipSize.Width /= mipLevel2;
            mipSize.Height /= mipLevel2;
            

            if (showAtlas)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(currentTilemap.TextureAtlas, mipSize, Color.White);
                spriteBatch.End();
            }
            else
            {
                DrawTilemap(gameTime, currentTilemap, mipSize);
            }


            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
            spriteBatch.Draw(rt, atlasSize, mipSize, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, String.Format("[1] MipmapPerSprite - ({0})", useMipmapPerSprite ? "ON" : "OFF"), new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(font, String.Format("[2] GenerateMipmap - ({0})", useGenerateBitmap ? "ON" : "OFF"), new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(font, String.Format("[3] {0}", showAtlas? "Show Tilemap" : "Show Atlas"), new Vector2(20, 60), Color.White);
            spriteBatch.DrawString(font, String.Format("[+/-] MipLevel - ({0})", mipLevel), new Vector2(20, 80), Color.White);
            spriteBatch.End();

        }
        
    }
}

# Aether.Graphics
Graphics library for [Kni](https://github.com/kniengine/kni) framework.

## Graphics

* 'Tilemap' - Draws a Tilemap texture.

example:

    // Initialize
    tilemap = Content.Load<Tilemap>("tilemap");

    // Draw
    Viewport viewport = GraphicsDevice.Viewport;
    
    tilemap.Effect.World = Matrix.Identity;
    tilemap.Effect.View = Matrix.Identity; // camera
    tilemap.Effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            
    spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, tilemap.Effect);
    spriteBatch.Draw(tilemap.TextureMap, mipSize, Color.White);
    spriteBatch.End();

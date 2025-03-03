# Aether.Graphics
Graphics library for [Kni](https://github.com/kniengine/kni) framework.

## Graphics

* 'TextureAtlas' - A TextureAtlas with a collection of sprites.

example:

    // Initialize
    textureAtlas = Content.Load<TextureAtlas>("atlas");

    // Draw
    Sprite sprite0 = textureAtlas.Sprites["spriteName0"];
    spriteBatch.Draw(sprite0, new Vector2(128, 128), Color.White);
    Sprite sprite1 = textureAtlas.Sprites["spriteName1"];
    spriteBatch.Draw(sprite1, new Vector2(256 , 128), Color.White);

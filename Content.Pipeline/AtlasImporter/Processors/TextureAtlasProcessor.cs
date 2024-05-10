#region License
//   Copyright 2016-2019 Kastellanos Nikolaos
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
#endregion

using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace nkast.Aether.Content.Pipeline
{
    [ContentProcessor(DisplayName = "AtlasProcessor - Aether")]
    public class AtlasProcessor : TextureProcessor, IContentProcessor
    {        
        private bool _mipmapsPerSprite = true;

#if WINDOWS
        // override InputType
        [Browsable(false)]
#endif
        Type IContentProcessor.InputType { get { return typeof(TextureAtlasContent); } }

#if WINDOWS
        // override OutputType
        [Browsable(false)]
#endif
        Type IContentProcessor.OutputType { get { return typeof(TextureAtlasContent); } }
        

        [DefaultValue(true)]
        public bool MipmapsPerSprite
        {
            get { return _mipmapsPerSprite; }
            set { _mipmapsPerSprite = value; }
        }

        public AtlasProcessor()
        {
        }
        
        object IContentProcessor.Process(object input, ContentProcessorContext context)
        {
            return Process((TextureAtlasContent)input, context);
        }

        public TextureAtlasContent Process(TextureAtlasContent input, ContentProcessorContext context)
        {
            if (MipmapsPerSprite && GenerateMipmaps)
                foreach (SpriteContent texture in input.SourceSprites)
                    texture.Texture.GenerateMipmaps(false);

            TextureAtlasContent output = input;
            
            if (GenerateMipmaps)
            {
                if (MipmapsPerSprite)
                {
                    int maxSpriteWidth = 1;
                    int maxSpriteHeight = 1;
                    foreach (SpriteContent sprite in input.SourceSprites)
                    {
                        MipmapChain face0 = sprite.Texture.Faces[0];
                        maxSpriteWidth = Math.Max(maxSpriteWidth, face0[0].Width);
                        maxSpriteHeight = Math.Max(maxSpriteHeight, face0[0].Height);
                    }

                    for (int mipLevel = 1; ; mipLevel++)
                    {
                        int mipLevel2 = (int)Math.Pow(2, mipLevel);
                        Rectangle size = new Rectangle(0, 0, input.Width, input.Height);
                        size.Width /= mipLevel2;
                        size.Height /= mipLevel2;

                        if ((maxSpriteWidth / mipLevel2) < 1 && (maxSpriteHeight / mipLevel2) < 1) break;

                        var mipmapBmp = new PixelBitmapContent<Color>(size.Width, size.Height);
                        foreach (SpriteContent sprite in input.DestinationSprites)
                        {
                            if (mipLevel >= sprite.Texture.Faces[0].Count) continue;
                            BitmapContent srcBmp = sprite.Texture.Faces[0][mipLevel];
                            Rectangle srcRect = new Rectangle(0, 0, srcBmp.Width, srcBmp.Height);
                            Rectangle destRect = sprite.Bounds;
                            destRect.X = (int)Math.Ceiling((float)destRect.X / mipLevel2);
                            destRect.Y = (int)Math.Ceiling((float)destRect.Y / mipLevel2);
                            destRect.Width = (int)(destRect.Width / mipLevel2);
                            destRect.Height = (int)(destRect.Height / mipLevel2);
                            if (destRect.Width > 1 && destRect.Height > 1)
                                BitmapContent.Copy(srcBmp, srcRect, mipmapBmp, destRect);
                        }
                        output.Texture.Mipmaps.Add(mipmapBmp);
                    }

                    MipmapChain outputFace0 = output.Texture.Faces[0];
                    while (outputFace0[outputFace0.Count - 1].Width > 1 || outputFace0[outputFace0.Count - 1].Height > 1)
                    {
                        BitmapContent lastMipmap = outputFace0[outputFace0.Count - 1];
                        int w = Math.Max(1, lastMipmap.Width/2);
                        int h = Math.Max(1, lastMipmap.Height/2);
                        var mipmapBmp = new PixelBitmapContent<Color>(w, h);
                        //PixelBitmapContent<Color>.Copy(lastMipmap, mipmapBmp);
                        output.Texture.Mipmaps.Add(mipmapBmp);
                    }
                }
                else
                {
                    output.Texture.GenerateMipmaps(false);
                }
            }
            
            // Workaround MonoGame TextureProcessor bug.
            // MonoGame TextureProcessor overwrites existing mipmaps.
            if (MipmapsPerSprite && GenerateMipmaps)
            {
                GenerateMipmaps = false;
                base.Process(output.Texture, context);
                GenerateMipmaps = true;
            }
            else
            {
                base.Process(output.Texture, context);
            }
            
            return output;
        }
        
    }
}
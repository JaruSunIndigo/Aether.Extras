#region License
//   Copyright 2021 Kastellanos Nikolaos
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace nkast.Aether.Content.Pipeline
{
    [ContentImporter(".tmx", DisplayName = "Tilemap Importer - Aether", DefaultProcessor = "TilemapProcessor")]
    public class TilemapImporter : ContentImporter<TilemapContent>
    {
        public override TilemapContent Import(string filename, ContentImporterContext context)
        {
            TilemapContent output;

            if (Path.GetExtension(filename) == ".tmx")
                output = ImportTMX(filename, context);
            else
                throw new InvalidContentException("File type not supported");

            TilemapContent.PackTiles(output, output.TileWidth, output.TileHeight);
            TilemapContent.RenderAtlas(output);
            TilemapContent.RenderMap(output);

            return output;
        }
        
        private static TilemapContent ImportTMX(string filename, ContentImporterContext context)
        {
            TilemapContent output = new TilemapContent();
            output.Identity = new ContentIdentity(filename);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            XmlElement map = xmlDoc.DocumentElement;
            string orientation = map.GetAttribute("orientation");
            if (orientation != "orthogonal")
                throw new InvalidContentException("Invalid orientation. Only 'orthogonal' is supported for atlases.");
            output.Renderorder = map.GetAttribute("renderorder");
            output.MapColumns = map.GetAttributeAsInt("width").Value;
            output.MapRows = map.GetAttributeAsInt("height").Value;
            output.TileWidth = map.GetAttributeAsInt("tilewidth").Value;
            output.TileHeight = map.GetAttributeAsInt("tileheight").Value;
            output.Width = output.MapColumns * output.TileWidth;
            output.Height = output.MapRows * output.TileHeight;

            XmlNode tilesetNode = map["tileset"];
            output.Firstgid = tilesetNode.GetAttributeAsInt("firstgid").Value;

            if (tilesetNode.Attributes["source"] != null)
            {
                string tsxFilename = tilesetNode.Attributes["source"].Value;
                string baseDirectory = Path.GetDirectoryName(filename);
                tsxFilename = Path.Combine(baseDirectory, tsxFilename);

                TilesetContent tileset = ImportTSX(tsxFilename, context);
                output.Tileset = tileset;
                context.AddDependency(tsxFilename);
            }
            else
            {
                string rootDirectory = Path.GetDirectoryName(filename);
                TilesetContent tileset = ImportTileset(tilesetNode, context, rootDirectory);
                output.Tileset = tileset;
            }

            XmlNode layerNode = map["layer"];
            int layerColumns = Convert.ToInt32(map.Attributes["width"].Value, CultureInfo.InvariantCulture);
            int layerRows = Convert.ToInt32(map.Attributes["height"].Value, CultureInfo.InvariantCulture);
            output.LayerColumns = layerColumns;
            output.LayerRows = layerRows;

            XmlNode layerDataNode = layerNode["data"];
            string encoding = layerDataNode.Attributes["encoding"].Value;
            if (encoding != "csv")
                throw new InvalidContentException("Invalid encoding. Only 'csv' is supported for data.");
            string data = layerDataNode.InnerText;
            string[] dataStringList = data.Split(',');
            int[] mapData = new int[dataStringList.Length];
            for (int i = 0; i < dataStringList.Length; i++)
                mapData[i] = Convert.ToInt32(dataStringList[i].Trim(), CultureInfo.InvariantCulture);
            output.MapData = mapData;

            return output;
        }

        private static TilesetContent ImportTSX(string tsxFilename, ContentImporterContext context)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(tsxFilename);
            XmlNode tilesetNode = xmlDoc.DocumentElement;
            string baseDirectory = Path.GetDirectoryName(tsxFilename);
            return ImportTileset(tilesetNode, context, baseDirectory);
        }

        private static TilesetContent ImportTileset(XmlNode tilesetNode, ContentImporterContext context, string baseDirectory)
        {
            TilesetContent tileset = new TilesetContent();

            if (tilesetNode["tileoffset"] != null)
                throw new InvalidContentException("tileoffset is not supported.");

            tileset.TileWidth = tilesetNode.GetAttributeAsInt("tilewidth").Value;
            tileset.tileHeight = tilesetNode.GetAttributeAsInt("tileheight").Value;

            BitmapContent bm = new PixelBitmapContent<Color>(tileset.TileWidth, tileset.tileHeight);
            Texture2DContent mt = new Texture2DContent();
            mt.Faces[0].Add(bm);
            mt.Name = "null";
            TileContent nullTile = new TileContent();
            nullTile.Tileset = tileset;
            nullTile.Id = -1;
            nullTile.SrcTexture = mt;
            nullTile.SrcBounds.Location = Point.Zero;
            nullTile.SrcBounds.Width = mt.Mipmaps[0].Width;
            nullTile.SrcBounds.Height = mt.Mipmaps[0].Height;
            nullTile.DstBounds.Location = Point.Zero;
            nullTile.DstBounds.Width = tileset.TileWidth;
            nullTile.DstBounds.Height = tileset.tileHeight;

            tileset.SourceTiles.Add(nullTile);

            foreach (XmlNode tileNode in tilesetNode.ChildNodes)
            {
                if (tileNode.Name != "tile") continue;
                int tileId = tileNode.GetAttributeAsInt("id").Value;

                XmlNode imageNode = tileNode["image"];

                //string format = GetAttribute(imageNode, "format");
                string imageSource = imageNode.GetAttribute("source");
                string fullImageSource = Path.Combine(baseDirectory, imageSource);
                TextureImporter txImporter = new TextureImporter();
                Texture2DContent textureContent = (Texture2DContent)txImporter.Import(fullImageSource, context);
                textureContent.Name = Path.GetFileNameWithoutExtension(fullImageSource);

                TileContent source = new TileContent();
                source.Tileset = tileset;
                source.Id = tileId;
                source.SrcTexture = textureContent;
                source.SrcBounds.Location = Point.Zero;
                source.SrcBounds.Width  = textureContent.Mipmaps[0].Width;
                source.SrcBounds.Height = textureContent.Mipmaps[0].Height;
                source.DstBounds.Location = Point.Zero;
                source.DstBounds.Width = textureContent.Mipmaps[0].Width;
                source.DstBounds.Height = textureContent.Mipmaps[0].Height;

                Color? transKeyColor = imageNode.GetAttributeAsColor("trans");
                if (transKeyColor != null)
                    foreach (MipmapChain mips in textureContent.Faces)
                        foreach (BitmapContent mip in mips)
                            ((PixelBitmapContent<Color>)mip).ReplaceColor(transKeyColor.Value, Color.Transparent);

                if (tileId != tileset.SourceTiles.Count-1)
                    throw new InvalidContentException("Invalid id");

                tileset.SourceTiles.Add(source);
            }

            return tileset;
        }
        
    }

}

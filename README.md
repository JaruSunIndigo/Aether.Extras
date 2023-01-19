# Aether.Extras
Content Importers and Shaders for [MonoGame](https://github.com/MonoGame/MonoGame) and [Kni](https://github.com/kniengine/kni) framework.

## Content Importers

* 'Animation' - Import animations from a Model.
* 'GPU AnimatedModel' - Import an animated Model.
* 'CPU AnimatedModel' - Import an animated Model to be animated by the CPU. Based on DynamicModelProcessor, the imported asset is of type Microsoft.Xna.Framework.Graphics.Model where the VertexBuffer is replaced by a CpuAnimatedVertexBuffer. CpuAnimatedVertexBuffer inherits from DynamicVertexBuffer.
* 'DDS Importer' - Import of DDS files (images, Cubemaps). Supports importing of DDS with DTX format.
* 'RawModelProcessor' - Import 3D Models with a raw copy of Vertex/Index data for platforms that don't support GetData().
* 'DynamicModel' - Base Processor to customize the build in Model. It allows to modify
VertexBuffer & IndexBuffers, make them Dynamic and WriteOnly.
* 'VoxelModelImporter' - Import .vox files as 3D Models.
* 'AtlasImporter' - Import sprite atlas. Supports .tmx files. Mipmaps are generated individually for each sprite, no color-leak.
* 'TilemapImporter' - Import tilemap files. Supports .tmx files.

## nkast.Aether.Animation

Play animated 3D models and support for CPU animation.
CPU animation is optimized using unsafe code, writing directly to mapped VertexBuffer memory using reflection (DirectX). 

## nkast.Aether.Graphics

Draw Atlas sprites from TextureAtlas.
Draw Tilemaps.

## nkast.Aether.Shaders

* 'FXAA' - MonoGame port of NVIDIA's FXAA 3.11 shader.
* 'Deferred' - Deferred rendering.
* 'InfiniteGrid' - Draws an Infinite Grid.
* 'Tilemap' - Draws a Tilemap texture.

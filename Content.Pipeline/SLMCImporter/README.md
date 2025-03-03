# Aether.Content.Pipeline
Content Importers for [Kni](https://github.com/kniengine/kni) framework.

## Content Importers

* 'SLMCImporter' - Import an .slmc file. Compines color channels from multiple textures, into a single texture.

example:

<?xml version="1.0" encoding="UTF-8"?>
<channels>
   <image source="Channel0.png"/>
   <image source="Channel1.png"/>
   <image source="Channel2.png"/>
   <image source="Channel3.png"/>
</channels>

## Content Processors

* 'SLMCProcessor' - Process a TextureContent from SLMCImporter. Can convert BGRA4444 and generate Mipmaps.

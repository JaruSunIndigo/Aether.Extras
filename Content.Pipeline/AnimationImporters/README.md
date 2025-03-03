# Aether.Content.Pipeline
Content Importers for [Kni](https://github.com/kniengine/kni) framework.

## Content Importers

* 'Animation' - Import animations from a Model.

## Content Processors

* 'GPU AnimatedModel' - Import an animated Model.
* 'CPU AnimatedModel' - Import an animated Model to be animated by the CPU. Based on DynamicModelProcessor, the imported asset is of type Microsoft.Xna.Framework.Graphics.Model where the VertexBuffer is replaced by a CpuAnimatedVertexBuffer. CpuAnimatedVertexBuffer inherits from DynamicVertexBuffer.

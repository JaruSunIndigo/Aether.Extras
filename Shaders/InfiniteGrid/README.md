# Aether.Shaders
Shaders for [Kni](https://github.com/kniengine/kni) framework.

## Shaders

* 'InfiniteGridEffect' - Draws an Infinite Grid.

## Game Components

* 'InfiniteGridComponent'

example:
       
    // Initialize
    gridComponent = new InfiniteGridComponent(GraphicsDevice, Content);
    Components.Add(gridComponent);
           
    // Update
    gridComponent.Projection = projection;
    gridComponent.View = view;
    gridComponent.EditMatrix = Matrix.Identity; // XY plane

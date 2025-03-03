# Aether.Graphics
Graphics library for [Kni](https://github.com/kniengine/kni) framework.

## Graphics

* nkast.Aether.Animation.Animations

Play animated 3D models. Support GPU and CPU animation.


## Example

-Import 3D model with GPU AnimatedModel or GPU AnimatedModel Processor. Use SkinnedEffect for GPU and BasicEffect for CPU based animation.

-Load as any 3D Model:

	_model = Content.Load<Model>("animatedModel");

-Load the Animations from model:

	_animations = _model.GetAnimations();
	var clip = _animations.Clips["ClipName"];
        _animations.SetClip(clip);

-Update animation on every frame:

        _animations.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

-Draw GPU animation:

	foreach (ModelMesh mesh in _model.Meshes)
	{
		foreach (var meshPart in mesh.MeshParts)
		{
			meshPart.effect.SetBoneTransforms(_animations.AnimationTransforms);
			// set effect parameters, lights, etc          
		}
		mesh.Draw();
	}

-Draw CPU animation:

	foreach (ModelMesh mesh in _model.Meshes)
	{
		foreach (var meshPart in mesh.MeshParts)
		{
		       meshPart.UpdateVertices(animationPlayer.AnimationTransforms);
		       // set effect parameters, lights, etc
		}
		mesh.Draw();
	}





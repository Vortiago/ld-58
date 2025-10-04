using Godot;
using System;

public partial class PhotoFrame : MeshInstance3D
{
	[Signal] public delegate void FrameClickedEventHandler(Camera3D camera);

	[Export] public Texture2D FrameTexture { get; set; }
	[Export] public Color FrameColor { get; set; } = Colors.White;
	[Export] public ShaderMaterial HighlightMaterial { get; set; }

	private Camera3D _camera;
	private InteractableObject _interactableObject;

	public override void _Ready() {
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");

		_interactableObject.Model = this;
		if (HighlightMaterial != null)
		{
			_interactableObject.HighlightMaterial = HighlightMaterial;
		}
		_interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;

		// Apply frame color/texture if set
		if (FrameTexture != null || FrameColor != Colors.White)
		{
			var material = GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
			if (material != null)
			{
				material = (StandardMaterial3D)material.Duplicate();
				if (FrameTexture != null)
					material.AlbedoTexture = FrameTexture;
				material.AlbedoColor = FrameColor;
				SetSurfaceOverrideMaterial(0, material);
			}
		}
	}

	public override void _ExitTree() {
		_interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
	}

	private void OnInteractableObjectClicked() {
		GD.Print($"PhotoFrame {Name} clicked, switching to camera view");
		EmitSignal(SignalName.FrameClicked, _camera);
	}
}

using Godot;
using System;

public partial class Frame1 : MeshInstance3D
{
	[Signal] public delegate void FrameClickedEventHandler(Camera3D camera);
	
	[Node("Camera3D")]
	private Camera3D _camera;

	[Node("InteractableObject")]
	private InteractableObject _interactableObject;
	
	public override void _Ready() {
		_interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
	}

	public override void _ExitTree() {
		_interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
	}

	private void OnInteractableObjectClicked() {
		GD.Print("Frame clicked, switching to camera view");
		EmitSignal(SignalName.FrameClicked, _camera);
	}
}

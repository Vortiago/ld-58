using Godot;
using System;

public partial class Frame1 : MeshInstance3D
{
	[Signal] public delegate void FrameClickedEventHandler(Camera3D camera);
	
	private Camera3D _camera;
	
	public override void _Ready() {
		_camera = GetNode<Camera3D>("Camera3D");
	}
	
	public void OnArea3DInputEvent(Node camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shape) {
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left) {
			GD.Print("Frame clicked, switching to camera view");
			EmitSignal(SignalName.FrameClicked, _camera);
		}
	}
}

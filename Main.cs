using Godot;
using System;

public partial class Main : Node3D
{
	[Export] public Camera3D CameraBlue {get; set;}
	[Export] public Camera3D CameraRed {get; set;}
	
	public override void _Ready() {
		ConnectPhotoFrames();
	}
	
	private void ConnectPhotoFrames() {
		var redFrame = GetNode<Area3D>("PhotoFrames/Frame1/Area3D");
		redFrame.InputEvent += (camera, @event, pos, normal, shape) => {
			if (@event is InputEventMouseButton mouse && mouse.Pressed) {
				SwitchToView(0);
				GD.Print("Switching to 0");
			}
		};
		
		var blueFrame = GetNode<Area3D>("PhotoFrames/Frame2/Area3D");
		blueFrame.InputEvent += (camera, @event, pos, normal, shape) => {
			if (@event is InputEventMouseButton mouse && mouse.Pressed) {
				SwitchToView(1);
				GD.Print("Switching to 1");
			}
		};
	}
	
	private void SwitchToView(int viewIndex) {
		CameraBlue.Current = false;
		CameraRed.Current = false;
		
		switch (viewIndex) {
			case 0: { CameraRed.Current = true; break; }
			case 1: { CameraBlue.Current = true; break; }
		}
	}
}

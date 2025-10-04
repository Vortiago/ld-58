using Godot;
using System;

public partial class Main : Node3D
{
	[Export] public Camera3D CameraBlue {get; set;}
	[Export] public Camera3D CameraRed {get; set;}
	
	public override void _Ready() {
	}
	
	public void FrameClicked(Camera3D associatedCamera) {
		CameraBlue.Current = false;
		CameraRed.Current = false;
		
		associatedCamera.Current = true;
	}
}

using Godot;
using System;

public partial class Main : Node3D
{
	public void FrameClicked(Camera3D associatedCamera) {
		associatedCamera.MakeCurrent();

		// var allCameras = GetTree().GetNodesInGroup("FrameCameras");
		// foreach(Camera3D camera in allCameras) {
		// 	camera.Current = false;
		// }
		
		// associatedCamera.Current = true;
	}
}

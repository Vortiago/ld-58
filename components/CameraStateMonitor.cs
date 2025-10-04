using System;
using Godot;

[GlobalClass]
public partial class CameraStateMonitor : Node
{
	[Signal] public delegate void CameraActivatedEventHandler();
	[Signal] public delegate void CameraDeactivatedEventHandler();

	private Camera3D _camera;
	private bool _wasCurrentLastFrame = false;

	public override void _Ready()
	{
		_camera = GetParent<Camera3D>();
		if (_camera == null)
		{
			GD.PrintErr("CameraStateMonitor must be a child of a Camera3D node");
			QueueFree();
			return;
		}

		// Initialize state

		_wasCurrentLastFrame = _camera.Current;
	}

	public override void _Process(double delta)
	{
		if (_camera == null) return;

		bool isCurrentNow = _camera.Current;

		// Detect state change

		if (isCurrentNow && !_wasCurrentLastFrame)
		{
			// Camera just became active

			GD.Print($"{_camera.GetParent().Name}/{_camera.Name} activated");
			EmitSignal(SignalName.CameraActivated);
		}
		else if (!isCurrentNow && _wasCurrentLastFrame)
		{
			// Camera just became inactive

			GD.Print($"{_camera.GetParent().Name}/{_camera.Name} deactivated");
			EmitSignal(SignalName.CameraDeactivated);
		}

		_wasCurrentLastFrame = isCurrentNow;
	}
}

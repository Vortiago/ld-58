using System;
using Godot;

public partial class Main : Node3D
{
	private DialogSystem _dialogSystem;
	private PhotoFrame _initialFrame;

	public void FrameClicked(Camera3D associatedCamera)
	{
		associatedCamera.MakeCurrent();
	}

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("DialogSystem");
		_dialogSystem.DialogFinished += OnDialogFinished;

		// Find the initial frame (Frame1 - "us" frame) and activate it on game start
		_initialFrame = GetNode<PhotoFrame>("Hallway/PhotoFrames/Frame1");
		if (_initialFrame != null)
		{
			CallDeferred(MethodName.ActivateInitialFrame);
		}
	}

	private void ActivateInitialFrame()
	{
		_initialFrame.ActivateFrame();
	}

	private void OnDialogFinished()
	{
		GD.Print("Dialog finished - resuming game");
		// Resume game logic here
	}
}

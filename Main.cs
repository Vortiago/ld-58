using Godot;
using System;

public partial class Main : Node3D
{
	private DialogSystem _dialogSystem;

	public void FrameClicked(Camera3D associatedCamera)
	{
		associatedCamera.MakeCurrent();
	}

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("DialogSystem");
		_dialogSystem.DialogFinished += OnDialogFinished;
		StartOpeningCutscene();
	}

	private void StartOpeningCutscene()
	{
		string[] openingLines = {
		"I am Detective Holmes, and I've been called to investigate a most peculiar murder.",
		"The victim, Lord Ashworth, lies dead in this very hallway.",
		"But something strange has happened... I seem to be trapped within a portrait!",
		"Perhaps I can examine the scene by jumping between the other portraits...",
		"Click on the colored photo frames to investigate from different perspectives."
	};

		_dialogSystem.StartDialog("Detective Holmes", openingLines);
	}

	private void OnDialogFinished()
	{
		GD.Print("Dialog finished - resuming game");
		// Resume game logic here
	}
}

using System;
using Godot;

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
		CallDeferred(MethodName.StartOpeningCutscene);
	}

	private void StartOpeningCutscene()
	{
		string[] openingLines = {
		"What's that smell... blood? And why is the morning light so harsh today?",
		"Wait... Lord Blackwood's body! There, by the corner table!",
		"The other portraits are waking. I wonder if any of them saw anything last night. Time to do what I do best—investigate.",
		"Lady Margaret looks shaken. I should start there."
	};

		_dialogSystem.StartDialog(openingLines, "Inspector Crawford", String.Empty, true);
	}

	private void OnDialogFinished()
	{
		GD.Print("Dialog finished - resuming game");
		// Resume game logic here

	}
}

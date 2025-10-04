using System;
using Godot;

public partial class Main : Node3D
{
	private DialogSystem _dialogSystem;
	private PhotoFrame _initialFrame;
	private GameUI _gameUI;
	private ClueContainer _clueContainer;
	private EndGameDialog _endGameDialog;

	public void FrameClicked(Camera3D associatedCamera)
	{
		associatedCamera.MakeCurrent();
	}

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("DialogSystem");
		_dialogSystem.DialogFinished += OnDialogFinished;

		// Get UI system references
		_gameUI = GetNode<GameUI>("GameUI");
		_clueContainer = GetNode<ClueContainer>("ClueContainer");
		_endGameDialog = GetNode<EndGameDialog>("EndGameDialog");

		// Initialize GameUI with references to the dialog systems
		_gameUI.Initialize(_clueContainer, _endGameDialog);

		// Subscribe to EndGameDialog events
		_endGameDialog.AnswerSubmitted += OnAnswerSubmitted;

		// Find the initial frame (Frame1 - "us" frame) and activate it on game start
		_initialFrame = GetNode<PhotoFrame>("Hallway/PhotoFrames/Frame1");
		if (_initialFrame != null)
		{
			CallDeferred(MethodName.ActivateInitialFrame);
		}
	}

	public override void _ExitTree()
	{
		_dialogSystem.DialogFinished -= OnDialogFinished;
		_endGameDialog.AnswerSubmitted -= OnAnswerSubmitted;
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

	private void OnAnswerSubmitted(int whoAnswer, int whatAnswer, int whyAnswer)
	{
		GD.Print($"Answer submitted - Who: {whoAnswer}, What: {whatAnswer}, Why: {whyAnswer}");
		// TODO: Check if the answer is correct and show appropriate feedback
	}

	/// <summary>
	/// Adds a clue to the clue container.
	/// </summary>
	public void AddClue(string header, string body)
	{
		_clueContainer.CreateClueItem(header, body);
	}

	/// <summary>
	/// Unlocks an option in the end game dialog.
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	public void UnlockOption(int question, int option)
	{
		_endGameDialog.SetOptionAvailability(question, option, true);
	}

	/// <summary>
	/// Sets the text for an option in the end game dialog.
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	/// <param name="text">The text to display</param>
	public void SetOptionText(int question, int option, string text)
	{
		_endGameDialog.SetOptionText(question, option, text);
	}
}

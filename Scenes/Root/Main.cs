using System;
using Godot;

public partial class Main : Node3D
{
	private DialogSystem _dialogSystem;
	private PhotoFrame _initialFrame;
	private GameUI _gameUI;
	private ClueContainer _clueContainer;
	private EndGameDialog _endGameDialog;
	private StartScreen _startScreen;

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
		_startScreen = GetNode<StartScreen>("StartScreen");

		// Initialize GameUI with references to the dialog systems
		_gameUI.Initialize(_clueContainer, _endGameDialog);

		// Subscribe to events
		_endGameDialog.AnswerSubmitted += OnAnswerSubmitted;
		_startScreen.GameStarted += OnGameStarted;

		// Find the initial frame (Frame1 - "us" frame)
		_initialFrame = GetNode<PhotoFrame>("Hallway/PhotoFrames/Frame1");

		// Start with the start screen visible
		_startScreen.Show();
	}

	public override void _ExitTree()
	{
		_dialogSystem.DialogFinished -= OnDialogFinished;
		_endGameDialog.AnswerSubmitted -= OnAnswerSubmitted;
		_startScreen.GameStarted -= OnGameStarted;
	}

	private void OnGameStarted()
	{
		if (_initialFrame != null)
		{
			// Switch to Frame1's camera and trigger dialog
			Camera3D camera = _initialFrame.GetNode<Camera3D>("Camera3D");
			camera.MakeCurrent();
			_initialFrame.ActivateFrame();
		}
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

		// Return to start screen
		_startScreen.Show();
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

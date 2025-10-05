using System;
using Godot;

public partial class Main : Node3D
{
	private DialogSystem _dialogSystem;
	private InspectorCrawford _initialFrame;
	private GameUI _gameUI;
	private ClueContainer _clueContainer;
	private EndGameDialog _endGameDialog;
	private StartScreen _startScreen;

	// Character frame references for resetting
	private HouseKeeper _houseKeeper;
	private DrHenryMorrison _drHenryMorrison;
	private LadyBlackwood _ladyBlackwood;
	private YoungTimBlackwood _youngTimBlackwood;
	private EleanorHeartwell _eleanorHartwell;

	// Game state flags
	private bool _isShowingEndGameDialog = false;

	public void FrameClicked(Camera3D associatedCamera)
	{
		associatedCamera.MakeCurrent();
	}

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("DialogSystem");
		_dialogSystem.DialogFinished += OnDialogFinished;
		GD.Print("[DEBUG] Main._Ready() - Connected to DialogSystem.DialogFinished event");

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

		// Find all character frames for game state management
		_initialFrame = GetNode<InspectorCrawford>("Hallway/PhotoFrames/InspectorCrawford");
		_houseKeeper = GetNode<HouseKeeper>("Hallway/PhotoFrames/HouseKeeper");
		_drHenryMorrison = GetNode<DrHenryMorrison>("Hallway/PhotoFrames/DrHenryMorrison");
		_ladyBlackwood = GetNode<LadyBlackwood>("Hallway/PhotoFrames/LadyBlackwood");
		_youngTimBlackwood = GetNode<YoungTimBlackwood>("Hallway/PhotoFrames/YoungTimBlackwood");
		_eleanorHartwell = GetNode<EleanorHeartwell>("Hallway/PhotoFrames/EleanorHartwell");

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
		GD.Print($"[DEBUG] OnDialogFinished called. _isShowingEndGameDialog = {_isShowingEndGameDialog}");

		if (_isShowingEndGameDialog)
		{
			// End game dialog finished, reset and return to start screen
			GD.Print("[DEBUG] End game dialog finished - resetting game and showing start screen");
			_isShowingEndGameDialog = false;
			ResetGameState();
			_startScreen.Show();
			GD.Print("[DEBUG] StartScreen.Show() called");
		}
		else
		{
			GD.Print("Dialog finished - resuming game");
			// Resume normal game logic here
		}
	}

	private void OnAnswerSubmitted(int whoAnswer, int whatAnswer, int whyAnswer)
	{
		GD.Print($"Answer submitted - Who: {whoAnswer}, What: {whatAnswer}, Why: {whyAnswer}");

		// Hide the end game dialog
		_endGameDialog.Hide();
		GD.Print("[DEBUG] EndGameDialog hidden");

		// Validate answers and calculate score
		int correctAnswers = ValidateAnswers(whoAnswer, whatAnswer, whyAnswer);

		// Set flag to indicate we're showing end game dialog
		_isShowingEndGameDialog = true;
		GD.Print($"[DEBUG] _isShowingEndGameDialog set to true. Showing end game dialog with {correctAnswers} correct answers");

		// Show the appropriate end game dialog through Inspector Crawford
		_initialFrame.ShowEndGameDialog(correctAnswers);
		GD.Print("[DEBUG] ShowEndGameDialog called on InspectorCrawford");
	}

	private int ValidateAnswers(int whoAnswer, int whatAnswer, int whyAnswer)
	{
		// Correct answers from QUICK_REFERENCE.md:
		// WHO: Thomas Hartwell (index 2)
		// WHAT: Ornate Letter Opener (index 0)
		// WHY: Business Fraud/Embezzlement (index 1)

		int correctCount = 0;

		if (whoAnswer == 2) // Thomas Hartwell
			correctCount++;

		if (whatAnswer == 0) // Ornate Letter Opener
			correctCount++;

		if (whyAnswer == 1) // Business Fraud/Embezzlement
			correctCount++;

		GD.Print($"Player got {correctCount} out of 3 correct");
		return correctCount;
	}

	/// <summary>
	/// Adds a clue to the clue container.
	/// </summary>
	/// <param name="header">Clue header/title</param>
	/// <param name="body">Clue description</param>
	/// <param name="portrait">Optional portrait texture of the character providing the clue</param>
	public void AddClue(string header, string body, Texture2D portrait = null)
	{
		_clueContainer.CreateClueItem(header, body, portrait);
	}

	/// <summary>
	/// Shows and unlocks an option in the end game dialog.
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	public void UnlockOption(int question, int option)
	{
		_endGameDialog.SetOptionVisibility(question, option, true);
	}

	/// <summary>
	/// Sets the visibility of an option in the end game dialog.
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	/// <param name="visible">True to show, false to hide</param>
	public void SetOptionVisibility(int question, int option, bool visible)
	{
		_endGameDialog.SetOptionVisibility(question, option, visible);
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

	/// <summary>
	/// Resets the entire game state for a new playthrough.
	/// </summary>
	private void ResetGameState()
	{
		GD.Print("Resetting game state for new playthrough");

		// Reset all character frame visit counts
		_initialFrame.ResetState();
		_houseKeeper.ResetState();
		_drHenryMorrison.ResetState();
		_ladyBlackwood.ResetState();
		_youngTimBlackwood.ResetState();
		_eleanorHartwell.ResetState();

		// Clear all clues from the container
		_clueContainer.ClearAllClues();

		// Reset EndGameDialog options - hide all options initially
		for (int i = 0; i < 4; i++)
		{
			_endGameDialog.SetOptionVisibility(0, i, false); // Who options
			_endGameDialog.SetOptionVisibility(1, i, false); // What options
			_endGameDialog.SetOptionVisibility(2, i, false); // Why options
		}

		// Hide dialogs
		_endGameDialog.Hide();
		_clueContainer.Hide();
	}
}

using System;
using Godot;

public partial class InspectorCrawford : Node3D
{
	private Camera3D _camera;

	private InteractableObject _interactableObject;

	private CameraStateMonitor _cameraStateMonitor;

	private DialogSystem _dialogSystem;
	private Main _main;
	private int _visitCount = 0;

	// Dialog configuration

	private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
	{
		new DialogSystem.DialogLine("Morning light already? From my frame I can see... wait, what's that smell? Blood?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Good heavens! Lord Blackwood's body, there by the corner table! My position gives me a clear view of the entire scene.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The other portraits are stirring. As the only detective portrait in residence, this investigation falls to my expertise.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Time to interview my fellow portraits—we see everything from our frames. Lady Margaret's portrait hangs directly across from the scene. I should speak with her first.", DialogSystem.SpeakerSide.Left)
	};

	private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
	{
		new DialogSystem.DialogLine("From this vantage point, I can observe the entire hallway. The other portraits may have seen something I missed.", DialogSystem.SpeakerSide.Left)
	};

	// End game dialogs based on score

	private readonly DialogSystem.DialogLine[] _allWrongDialog = new[]
	{
		new DialogSystem.DialogLine("I'm afraid your deductions are entirely incorrect, Inspector. The evidence points in a completely different direction.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Perhaps we should review the testimonies more carefully. The portraits have seen more than they initially revealed.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Let us begin our investigation anew. The truth of Lord Blackwood's murder remains hidden in these painted halls.", DialogSystem.SpeakerSide.Left)
	};

	private readonly DialogSystem.DialogLine[] _oneCorrectDialog = new[]
	{
		new DialogSystem.DialogLine("You've identified one aspect correctly, but two crucial pieces of the puzzle still elude you.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The portraits whisper of details you may have overlooked. Sometimes the most obvious clues hide deeper truths.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("We're making progress, but the complete picture remains obscured. Let us re-examine the evidence with fresh eyes.", DialogSystem.SpeakerSide.Left)
	};

	private readonly DialogSystem.DialogLine[] _twoCorrectDialog = new[]
	{
		new DialogSystem.DialogLine("Excellent work! You've correctly identified two key elements of this case. Just one more piece remains.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("You're very close to the truth now. The portraits nod in agreement with most of your deductions.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("With a bit more consideration, the final piece will fall into place. The answer is within your grasp!", DialogSystem.SpeakerSide.Left)
	};

	private readonly DialogSystem.DialogLine[] _allCorrectDialog = new[]
	{
		new DialogSystem.DialogLine("Brilliant deduction! You've solved it perfectly. Thomas Hartwell killed Lord Edgar with the ornate letter opener to hide his embezzlement.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The £50,000 theft was about to be exposed. In desperation, Thomas grabbed Edgar's own letter opener and struck.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The 'T.H.' handkerchief, the financial papers, Eleanor's protective lies—it all points to Thomas Hartwell.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Justice shall be served. The portraits of this hallway bear witness to your masterful investigation, Inspector Crawford.", DialogSystem.SpeakerSide.Left)
	};

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");
		_cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");


		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_main = GetNode<Main>("/root/Main");
		_interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
		_cameraStateMonitor.CameraActivated += OnCameraActivated;
		_cameraStateMonitor.CameraDeactivated += OnCameraDeactivated;
	}

	public override void _ExitTree()
	{
		_interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
		_cameraStateMonitor.CameraActivated -= OnCameraActivated;
		_cameraStateMonitor.CameraDeactivated -= OnCameraDeactivated;
	}

	private void OnInteractableObjectClicked()
	{
		_camera.MakeCurrent();
		ActivateFrame();
	}

	private void OnCameraActivated()
	{
		// Disable collision when this frame's camera is active

		_interactableObject.SetCollisionEnabled(false);
	}

	private void OnCameraDeactivated()
	{
		// Re-enable collision when this frame's camera is inactive

		_interactableObject.SetCollisionEnabled(true);
	}

	public void ActivateFrame()
	{
		_visitCount++;

		// Set up initial mystery options on first visit

		if (_visitCount == 1)
		{
			// Initialize Who options text (all hidden by default) - Max 4 options

			_main.SetOptionText(0, 0, "Lady Margaret Blackwood");
			_main.SetOptionText(0, 1, "Dr. Victor Pemberton");
			_main.SetOptionText(0, 2, "Thomas Hartwell");
			_main.SetOptionText(0, 3, "Miss Catherine Ashworth");

			// Initialize What (weapon) options text - Max 4 options

			_main.SetOptionText(1, 0, "Ornate Letter Opener");
			_main.SetOptionText(1, 1, "Poison (Digitalis)");
			_main.SetOptionText(1, 2, "Heavy Candlestick");
			_main.SetOptionText(1, 3, "Silk Scarf (Strangulation)");

			// Initialize Why (motive) options text - Max 4 options

			_main.SetOptionText(2, 0, "Inheritance Money");
			_main.SetOptionText(2, 1, "Business Fraud/Embezzlement");
			_main.SetOptionText(2, 2, "Blackmail Secret");
			_main.SetOptionText(2, 3, "Medical Cover-up");

			// Unlock Lady Margaret as initial suspect option (she's the wife, always a suspect)

			_main.UnlockOption(0, 0);

			// Unlock Inheritance as initial motive (common motive in murders)

			_main.UnlockOption(2, 0);
		}

		DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			_dialogSystem.StartDialog(dialogToShow, null, null);
		}
	}

	public void ShowEndGameDialog(int correctAnswers)
	{
		GD.Print($"[DEBUG] InspectorCrawford.ShowEndGameDialog called with {correctAnswers} correct answers");

		// Select the appropriate dialog based on score

		DialogSystem.DialogLine[] dialogToShow = correctAnswers switch
		{
			0 => _allWrongDialog,
			1 => _oneCorrectDialog,
			2 => _twoCorrectDialog,
			3 => _allCorrectDialog,
			_ => _allWrongDialog
		};

		GD.Print($"[DEBUG] Selected dialog with {dialogToShow.Length} lines");

		// Switch to Inspector's camera and show the dialog

		_camera.MakeCurrent();
		GD.Print("[DEBUG] Inspector camera made current");

		_dialogSystem.StartDialog(dialogToShow, null, null);
		GD.Print("[DEBUG] DialogSystem.StartDialog called");
	}

	public void ResetState()
	{
		_visitCount = 0;
	}
}

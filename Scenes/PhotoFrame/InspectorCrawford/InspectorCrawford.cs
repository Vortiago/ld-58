using Godot;
using System;

public partial class InspectorCrawford : Node3D
{
	[Node("Camera3D")]
	private Camera3D _camera;

	[Node("InteractableObject")]
	private InteractableObject _interactableObject;

	[Node("Camera3D/CameraStateMonitor")]
	private CameraStateMonitor _cameraStateMonitor;

	private DialogSystem _dialogSystem;
	private Main _main;
	private int _visitCount = 0;

	// Dialog configuration
	private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
	{
		new DialogSystem.DialogLine("What's that smell... blood? And why is the morning light so harsh today?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Wait... Lord Blackwood's body! There, by the corner table!", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The other portraits are waking. I wonder if any of them saw anything last night. Time to do what I do best—investigate.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Lady Margaret looks shaken. I should start there.", DialogSystem.SpeakerSide.Left)
	};

	private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
	{
		new DialogSystem.DialogLine("I do hope I can solve this case...", DialogSystem.SpeakerSide.Left)
	};

	public override void _Ready()
	{
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
			// Initialize Who options text (all hidden by default)
			_main.SetOptionText(0, 0, "Lady Margaret Blackwood");
			_main.SetOptionText(0, 1, "Dr. Victor Pemberton");
			_main.SetOptionText(0, 2, "Thomas Hartwell");
			_main.SetOptionText(0, 3, "Miss Catherine Ashworth");

			// Initialize What (weapon) options text
			_main.SetOptionText(1, 0, "Ornate Letter Opener");
			_main.SetOptionText(1, 1, "Poison (Digitalis)");
			_main.SetOptionText(1, 2, "Heavy Candlestick");
			_main.SetOptionText(1, 3, "Silk Scarf (Strangulation)");

			// Initialize Why (motive) options text
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
}

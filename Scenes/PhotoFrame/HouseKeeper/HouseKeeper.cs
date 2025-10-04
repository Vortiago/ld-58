using System;
using Godot;

public partial class HouseKeeper : Node3D
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
	private Texture2D _sarahMillsPortrait;

	// Dialog configuration - First Visit
	private readonly string[] _firstVisitDialog = new[]
	{
		"Sarah, your portrait has hung here for many years. Tell me what you saw last night.",
		"Inspector, it was dreadful! From my frame, I heard shouting—Lord Edgar's voice, angry and desperate.",
		"What time did this happen?",
		"Just after eleven-thirty, sir. I watched from my portrait as shadows moved near the corner table...",
		"Shadows? Did you see who it was?",
		"Mr. Hartwell! He came rushing past, right beneath where I hang. He looked panicked, wouldn't look up. And Inspector—there was blood on his sleeve!",
		"Blood on his sleeve? You're absolutely certain it was Thomas Hartwell?",
		"No doubt in my mind, sir. I've been painted in this house for twenty years—I know every face that walks these halls.",
		"From your vantage point, did you notice anything else? Any evidence?",
		"Yes! From my frame's angle, I could see by the plant stand—there's a handkerchief there, monogrammed, looks expensive.",
		"A handkerchief? What were the initials?",
		"'T.H.', sir. Same as Mr. Hartwell's. And it had blood on it too—I could see the dark stains even in the dim light.",
		"Excellent observation, Sarah. Anything else from what you could see?",
		"Only that Lord Edgar was a good man, Inspector. From up here, I've watched this household for decades. You must find who did this terrible thing."
	};

	// Dialog configuration - Subsequent Visits
	private readonly string[] _subsequentVisitDialog = new[]
	{
		"Sarah, is there anything else you saw from your frame?",
		"Only what I've told you, Inspector. Mr. Hartwell rushing past with blood on his sleeve—the image is burned into my painted memory."
	};

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_main = GetNode<Main>("/root/Main");
		_sarahMillsPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/HouseKeeper/SarahMills.png");
		_interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
		_cameraStateMonitor.CameraActivated += OnCameraActivated;
		_cameraStateMonitor.CameraDeactivated += OnCameraDeactivated;
		_dialogSystem.DialogFinished += OnDialogFinished;
	}

	public override void _ExitTree()
	{
		_interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
		_cameraStateMonitor.CameraActivated -= OnCameraActivated;
		_cameraStateMonitor.CameraDeactivated -= OnCameraDeactivated;
		_dialogSystem.DialogFinished -= OnDialogFinished;
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

		string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			// Alternate speakers: Inspector Crawford (left) speaks first (even indices)
			bool isLeftSpeaking = true;
			_dialogSystem.StartDialog(dialogToShow, "Sarah Mills", _sarahMillsPortrait, isLeftSpeaking);
		}
	}

	private void OnDialogFinished()
	{
		// Add handkerchief clue after first visit
		if (_visitCount == 1)
		{
			_main.AddClue(
				"Handkerchief — 'T.H.' Monogram",
				"An expensive silk handkerchief with blood stains and the monogram 'T.H.' in gold thread. Dropped near the potted plant in the escape route. 'T.H.' = Thomas Hartwell. Has blood on it (matches Sarah's testimony about blood on his sleeve). Expensive item he wouldn't normally leave behind (shows panic)."
			);
		}
	}
}

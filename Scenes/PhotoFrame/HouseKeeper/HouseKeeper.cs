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
	private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
	{
		new DialogSystem.DialogLine("Sarah, your portrait has hung here for many years. Tell me what you saw last night.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Inspector, it was dreadful! From my frame, I heard shouting—Lord Edgar's voice, angry and desperate.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("What time did this happen?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Just after eleven-thirty, sir. I watched from my portrait as shadows moved near the corner table... multiple people, I think, though the light was so dim.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("Multiple people? Could you identify any of them?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Someone came rushing past, right beneath where I hang. Moving fast, panicked-like. The dinner guests had all been in the drawing room earlier—could have been any of them, I suppose.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("Did you notice any distinguishing features?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("The lighting was poor, sir. But from my frame's angle, I could see by the plant stand—there's a handkerchief there, monogrammed, looks expensive.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("A handkerchief? What were the initials?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("'T.H.', sir. Gold thread embroidery, very fine work. And it had dark stains on it—looked like blood in the dim light.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("'T.H.'... that could be significant. Anything else you observed?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Only that Lord Edgar was a good man, Inspector. From up here, I've watched this household for decades. So many guests tonight, so many secrets. You must find who did this terrible thing.", DialogSystem.SpeakerSide.Right)
	};

	// Dialog configuration - Subsequent Visits
	private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
	{
		new DialogSystem.DialogLine("Sarah, is there anything else you saw from your frame?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Only what I've told you, Inspector. Someone rushing past in the darkness, and that handkerchief with the 'T.H.' monogram—the image is burned into my painted memory.", DialogSystem.SpeakerSide.Right)
	};

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_main = GetNode<Main>("/root/Main");
		_sarahMillsPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/HouseKeeper/SarahMills.png");
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

		// Add clues and unlock options on first visit
		if (_visitCount == 1)
		{
			// Add handkerchief clue (evidence)
			_main.AddClue(
				"Handkerchief — 'T.H.' Monogram",
				"An expensive silk handkerchief with blood stains and the monogram 'T.H.' in gold thread. Dropped near the potted plant in the escape route. Could belong to Thomas Hartwell (business partner) or Timothy Hartwell (Lord Edgar's nephew who lives in London). Expensive item suggesting upper-class owner. Blood stains indicate involvement in the crime.",
				_sarahMillsPortrait
			);

			// Add red herring clue about multiple suspects
			_main.AddClue(
				"Multiple Suspects (Sarah's Account)",
				"Sarah Mills reports seeing 'multiple people' moving in the shadows and someone 'rushing past' but couldn't identify who due to poor lighting. The dinner guests - any of them could have been in the hallway. This ambiguous testimony doesn't eliminate any suspects.",
				_sarahMillsPortrait
			);

			// Unlock Thomas Hartwell as suspect (T.H. handkerchief points to him)
			_main.SetOptionText(0, 2, "Thomas Hartwell"); // Who option 3
			_main.UnlockOption(0, 2);
		}

		DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			_dialogSystem.StartDialog(dialogToShow, "Sarah Mills", _sarahMillsPortrait);
		}
	}
}

using System;
using Godot;

public partial class HouseKeeper : Node3D
{
	private Camera3D _camera;

	private InteractableObject _interactableObject;

	private CameraStateMonitor _cameraStateMonitor;

	private DialogSystem _dialogSystem;
	private Main _main;
	private int _visitCount = 0;
	private Texture2D _sarahMillsPortrait;

	// Dialog configuration - First Visit
	private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
	{
		new DialogSystem.DialogLine("Sarah, you've watched over this hallway for many years. What did you observe last night?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Inspector, from my position here, I have a perfect view of the lower hallway. I heard shouting—Lord Edgar's voice, angry and desperate. We portraits share what we see, you know.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("What time did this happen? Can you be certain?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Around eleven-thirty, I believe—though we portraits don't always pay close attention to time. I've watched over this house since 1888, three generations now. Last night, shadows moved near the corner table... multiple figures.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("Multiple people? Could you identify any of them from your vantage point?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Someone rushed past, right beneath my frame. Moving fast, breathing hard. From my angle, I mostly see feet and shadows. The other portraits might have seen their face—we often discuss what we observe.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("Did you notice any distinguishing features?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("My position gives me an excellent view of the plant stand. There's a handkerchief there—monogrammed, expensive silk. The portrait of the late Lord Blackwood mentioned seeing it dropped during the commotion.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("A handkerchief? What were the initials?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("'T.H.', gold thread embroidery. Beautiful work. Dark stains on it too—blood perhaps. Though young Timothy's governess mentioned he'd had a nosebleed earlier that evening. Strange coincidence.", DialogSystem.SpeakerSide.Right),
		new DialogSystem.DialogLine("'T.H.'... Thomas Hartwell, or perhaps...?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("Could be Thomas, could be young Timothy Hartwell—he's Edgar's nephew visiting from London. Both were here last night. We portraits know all the family connections, Inspector. So many secrets painted on these walls.", DialogSystem.SpeakerSide.Right)
	};

	// Dialog configuration - Subsequent Visits
	private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
	{
		new DialogSystem.DialogLine("Sarah, is there anything else from your perspective?", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("From my frame, I see what I see, Inspector. That handkerchief with 'T.H.'—it troubles me. The other portraits whisper about it. Some say Thomas, some say Timothy. We portraits gossip terribly, you know.", DialogSystem.SpeakerSide.Right)
	};

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");
		_cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");

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
			// Add handkerchief clue (evidence - now ambiguous)
			_main.AddClue(
				"Handkerchief — 'T.H.' Monogram",
				"Expensive silk handkerchief with gold thread monogram 'T.H.' found by the plant stand. Has dark stains (blood or possibly from young Timothy's reported nosebleed earlier). Could belong to: Thomas Hartwell (business partner), Timothy Hartwell (Edgar's visiting nephew from London), or even Lord Edgar's late brother Theodore Hartwell's effects. Multiple portraits confirm seeing it dropped during the struggle.",
				_sarahMillsPortrait
			);

			// Add clue about multiple figures
			_main.AddClue(
				"Multiple Figures Observed",
				"Sarah's portrait position shows 'multiple figures' near the crime scene. Other portraits corroborate seeing more than one person, though accounts differ. Edgar's own words 'How could THEY do this?' suggest multiple conspirators. The shadows moved in ways suggesting coordinated action. However, portraits admit the dim lighting makes counting difficult.",
				_sarahMillsPortrait
			);

			// Add portrait gossip clue (creating doubt)
			_main.AddClue(
				"Portrait Network Intelligence",
				"The portraits share information constantly—'We portraits gossip terribly,' Sarah admits. They've observed this family for generations and know all the secrets. Different portraits offer conflicting theories about the 'T.H.' handkerchief owner. Their collective knowledge suggests deeper conspiracies than a single murderer.",
				_sarahMillsPortrait
			);

			// Unlock Thomas Hartwell as suspect
			_main.SetOptionText(0, 2, "Thomas Hartwell"); // Who option 3
			_main.UnlockOption(0, 2);

			// Note: Young Timothy option removed (limited to 4 options per question)
		}

		DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			_dialogSystem.StartDialog(dialogToShow, "Sarah Mills", _sarahMillsPortrait);
		}
	}

	public void ResetState()
	{
		_visitCount = 0;
	}
}

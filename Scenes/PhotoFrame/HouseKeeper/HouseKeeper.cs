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
	private int _visitCount = 0;
	private Texture2D _ladyMargaretPortrait;

	// Dialog configuration - First Visit
	private readonly string[] _firstVisitDialog = new[]
	{
		"Lady Margaret, I understand this is difficult. Can you tell me what you saw that night?",
		"It… it was chaos. I stepped out of the dining room just after Lord Edgar called for more brandy. There, by the corner table, I saw him—slumped, pale as marble.",
		"Did you notice anything before you saw him fall?",
		"Yes. Mud on the carpet. Someone had trailed it from the side corridor. I frowned, returned inside to fetch a cloth, and when I came back…",
		"Go on.",
		"…he was gone. Only that ornate letter opener remained, stained.",
		"Did you glimpse anyone fleeing?",
		"I caught movement in the shadow—someone slender, cloak sweeping. I thought… I thought it was Thomas Hartwell. He looked… unsettled.",
		"Thank you, Lady Margaret. Anything else?",
		"Edgar had been uneasy all evening. He muttered about betrayal—business dealings gone wrong. He showed me papers half-hidden under the table…",
		"We'll retrieve those documents, ma'am. You've been most helpful.",
		"I only pray we uncover the truth."
	};

	// Dialog configuration - Subsequent Visits
	private readonly string[] _subsequentVisitDialog = new[]
	{
		"Lady Margaret, any final thoughts?",
		"Only that truth finds its way, one way or another."
	};

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_ladyMargaretPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/HouseKeeper/SarahMills.png");
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

		string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			// Alternate speakers: Inspector Crawford (left) speaks first (even indices)
			bool isLeftSpeaking = true;
			_dialogSystem.StartDialog(dialogToShow, "Lady Margaret Blackwood", _ladyMargaretPortrait, isLeftSpeaking);
		}
	}
}

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
	private int _visitCount = 0;

	// Dialog configuration
	private readonly string[] _firstVisitDialog = new[]
	{
		"What's that smell... blood? And why is the morning light so harsh today?",
		"Wait... Lord Blackwood's body! There, by the corner table!",
		"The other portraits are waking. I wonder if any of them saw anything last night. Time to do what I do best—investigate.",
		"Lady Margaret looks shaken. I should start there."
	};

	private readonly string[] _subsequentVisitDialog = new[]
	{
		"I do hope I can solve this case..."
	};

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
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
			_dialogSystem.StartDialog(dialogToShow, null, null, true);
		}
	}
}

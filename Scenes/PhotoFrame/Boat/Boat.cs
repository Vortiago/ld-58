using System;
using Godot;

public partial class Boat : Node3D
{
	private Camera3D _camera;
	private InteractableObject _interactableObject;
	private CameraStateMonitor _cameraStateMonitor;
	private DialogSystem _dialogSystem;
	private Texture2D _portraitTexture;

	// Quirky dialog for the boat frame
	private readonly DialogSystem.DialogLine[] _dialog = new[]
	{
		new DialogSystem.DialogLine("Ah, to sail away from all this murder business...", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("*The painted waves seem to beckon*", DialogSystem.SpeakerSide.Right)
	};

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");
		_cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_portraitTexture = GD.Load<Texture2D>("res://Scenes/PhotoFrame/Boat/boat.png");

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

	private void ActivateFrame()
	{
		_dialogSystem.StartDialog(_dialog, "Sailboat", _portraitTexture);
	}

	private void OnCameraActivated()
	{
		_interactableObject.SetCollisionEnabled(false);
	}

	private void OnCameraDeactivated()
	{
		_interactableObject.SetCollisionEnabled(true);
	}
}
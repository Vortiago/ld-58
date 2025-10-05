using System;
using Godot;

public partial class Books : Node3D
{
	private Camera3D _camera;
	private InteractableObject _interactableObject;
	private CameraStateMonitor _cameraStateMonitor;
	private DialogSystem _dialogSystem;
	private Texture2D _portraitTexture;

	// Quirky dialog for the books frame
	private readonly DialogSystem.DialogLine[] _dialog = new[]
	{
		new DialogSystem.DialogLine("Crime and Punishment, Murder on the Orient Express... Someone has a theme going.", DialogSystem.SpeakerSide.Left),
		new DialogSystem.DialogLine("*You resist the urge to alphabetize them*", DialogSystem.SpeakerSide.Right)
	};

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");
		_cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		_portraitTexture = GD.Load<Texture2D>("res://Scenes/PhotoFrame/Books/books.png");

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
		_dialogSystem.StartDialog(_dialog, "Mystery Library", _portraitTexture);
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
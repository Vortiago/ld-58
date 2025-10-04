using Godot;
using System;

public partial class PhotoFrame : MeshInstance3D
{
	[Signal] public delegate void FrameClickedEventHandler(Camera3D camera);

	[Export] public Texture2D FrameTexture { get; set; }
	[Export] public Color FrameColor { get; set; } = Colors.White;
	[Export] public ShaderMaterial HighlightMaterial { get; set; }

	// Dialog configuration
	[ExportGroup("Dialog")]
	[Export] public string[] FirstVisitDialog { get; set; }
	[Export] public string[] SubsequentVisitDialog { get; set; }
	[Export] public string LeftSpeakerName { get; set; }
	[Export] public string RightSpeakerName { get; set; }
	[Export] public bool IsLeftSpeaking { get; set; } = true;

	private Camera3D _camera;
	private InteractableObject _interactableObject;
	private DialogSystem _dialogSystem;
	private int _visitCount = 0;

	public override void _Ready() {
		_camera = GetNode<Camera3D>("Camera3D");
		_interactableObject = GetNode<InteractableObject>("InteractableObject");
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");

		_interactableObject.Model = this;
		if (HighlightMaterial != null)
		{
			_interactableObject.HighlightMaterial = HighlightMaterial;
		}
		_interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;

		// Apply frame color/texture if set
		if (FrameTexture != null || FrameColor != Colors.White)
		{
			var material = GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
			if (material != null)
			{
				material = (StandardMaterial3D)material.Duplicate();
				if (FrameTexture != null)
					material.AlbedoTexture = FrameTexture;
				material.AlbedoColor = FrameColor;
				SetSurfaceOverrideMaterial(0, material);
			}
		}
	}

	public override void _ExitTree() {
		_interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
	}

	private void OnInteractableObjectClicked() {
		GD.Print($"PhotoFrame {Name} clicked, switching to camera view");
		EmitSignal(SignalName.FrameClicked, _camera);
		ActivateFrame();
	}

	public void ActivateFrame()
	{
		GD.Print($"ActivateFrame called on {Name}, visit count: {_visitCount}");

		if (_dialogSystem == null)
		{
			GD.PrintErr("DialogSystem is null, cannot trigger dialog");
			return;
		}

		string[] dialogToShow = _visitCount == 0 ? FirstVisitDialog : SubsequentVisitDialog;
		_visitCount++;

		if (dialogToShow != null && dialogToShow.Length > 0)
		{
			GD.Print($"Starting dialog with {dialogToShow.Length} lines");
			_dialogSystem.StartDialog(dialogToShow, LeftSpeakerName, RightSpeakerName, IsLeftSpeaking);
		}
		else
		{
			GD.Print($"No dialog to show (dialogToShow is {(dialogToShow == null ? "null" : "empty")})");
		}
	}
}

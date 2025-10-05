using System;
using Godot;

public partial class YoungTimBlackwood : Node3D
{
    private Camera3D _camera;

    private InteractableObject _interactableObject;

    private CameraStateMonitor _cameraStateMonitor;

    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;
    private Texture2D _youngTimothyPortrait;

    // Dialog configuration - First Visit
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Hello Timothy. What did you observe last night?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I heard shouting, then someone ran past my frame. The older portraits say I'm too young to understand.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Did you see who it was?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Only a shadow. Miss Catherine says I shouldn't talk about it. She protects me from things.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Protects you from what?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Papa had Uncle Timothy's portrait removed last month. I'm named after him. He died before I was born.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Why was the portrait removed?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The older portraits won't say. They whisper about 'family secrets painted in layers.' Papa looked scared when he did it.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Timothy, is there anything else you remember?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The portrait children keep asking if I saw who did it. I tell them about the shadow, but... Inspector, what if the shadow looked familiar? What if it was someone I know? Even portraits can have nightmares.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Who do you think it might have been?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I... I shouldn't guess. Miss Catherine says guessing can hurt people. But the handkerchief everyone talks about—'T.H.'—those are my initials too. Timothy Hartwell. Just like my dead uncle.", DialogSystem.SpeakerSide.Right)
    };

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");

        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _youngTimothyPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/YoungTimBlackwood/Young Timothy Blackwood.png");
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

        // Add clues on first visit
        if (_visitCount == 1)
        {
            // Add clue about the family blackmail secret
            _main.AddClue(
                "Family Secret - Uncle Timothy",
                "Young Timothy is named after his uncle Timothy Hartwell who died before he was born. Edgar had uncle's portrait removed last month looking 'scared.' The older portraits whisper about 'family secrets painted in layers' but refuse to explain. Was someone blackmailing Edgar about this family secret? The 'T.H.' handkerchief could belong to either Timothy.",
                _youngTimothyPortrait
            );

            // Add clue about the shadow witness
            _main.AddClue(
                "The Shadow Runner",
                "Timothy saw a shadow running past his frame after the shouting. He couldn't identify who it was. Miss Catherine told him not to talk about it—she 'protects him from things.' His reluctance to identify the runner is suspicious.",
                _youngTimothyPortrait
            );

            // Unlock Blackmail Secret as motive
            _main.UnlockOption(2, 2); // Why option 3: "Blackmail Secret"
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Young Timothy Blackwood", _youngTimothyPortrait);
        }
    }

    public void ResetState()
    {
        _visitCount = 0;
    }
}

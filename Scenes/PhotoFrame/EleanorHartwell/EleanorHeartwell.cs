using System;
using Godot;

public partial class EleanorHeartwell : Node3D
{

    private Camera3D _camera;

    private InteractableObject _interactableObject;

    private CameraStateMonitor _cameraStateMonitor;

    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;
    private Texture2D _eleanorHeartwellPortrait;

    // Dialog configuration - First Visit (nervous, protective, portrait perspective)
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Mrs. Hartwell, what did you observe about your husband's movements?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Thomas visits my frame often. Last night he seemed distracted. The other portraits noticed too.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Where was Thomas during the murder?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("He mentioned visiting the library. Though portraits in other wings saw him elsewhere. We share observations constantly.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("From your angle, could you see signs of strangulation?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("When I first looked, I saw no blood. Only Edgar slumped there.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("So you suspected a silk scarf?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Victorian gentlemen carry such scarves. Like in mystery novels. Though other portraits later mentioned a stab wound... scarves can hide marks.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits (resigned, cryptic)
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Mrs. Hartwell, is there anything else from your vantage point?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("My portrait watches the hallway every night, Inspector. The other frames whisper of what they've seen—so many theories, so many secrets.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Which theory do you believe?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("We portraits see everything and nothing. Thomas visits my frame often, but last night... even paintings can keep secrets from each other.", DialogSystem.SpeakerSide.Right)
    };

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");

        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _eleanorHeartwellPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/EleanorHartwell/Mrs. Eleanor Hartwell.png");
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

        // Add clues on first visit - Eleanor's observations create multiple possibilities
        if (_visitCount == 1)
        {
            // Add clue about portrait observations
            _main.AddClue(
                "Portrait Network Confusion",
                "Eleanor's portrait faces away from the main hallway—she must turn to observe. Multiple portraits report seeing Thomas in different locations: the Colonel saw him in the smoking room, Timothy's portrait in the hallway, others in the library. Portrait testimony conflicts, suggesting either Thomas moved frequently, multiple people were mistaken for Thomas, or portraits are unreliable witnesses.",
                _eleanorHeartwellPortrait
            );

            // Add clue about the muddy footprints with multiple sources
            _main.AddClue(
                "Multiple Footprint Sources",
                "Eleanor notes muddy prints could belong to: the gardener (left at 6 PM per kitchen portraits), Lord Blackwood himself (came from garden earlier), young Timothy (was playing outside), or any dinner guest who stepped out. Her frame position makes distinguishing details difficult. The abundance of explanations makes the evidence inconclusive.",
                _eleanorHeartwellPortrait
            );

            // Add clue about handedness ambiguity
            _main.AddClue(
                "Ambidextrous Possibility",
                "Eleanor insists Thomas writes right-handed, yet the guest register shows left-handed signature. She suggests: injury from carving knife accident at dinner (mentioned by Lady Margaret's portrait), artistic liberty in how they were painted, or that 'portraits see what they expect.' Could Thomas be ambidextrous? Or is someone forging his signature?",
                _eleanorHeartwellPortrait
            );

            // Add clue about portrait secrets
            _main.AddClue(
                "Portrait Secrets",
                "Eleanor reveals portraits can keep secrets from each other—they don't share everything despite constant gossip. Thomas visits Eleanor's frame often for 'portrait etiquette' but seemed 'distracted' last night. Her final words: 'even paintings can keep secrets.' Suggests portraits may be withholding information or protecting someone.",
                _eleanorHeartwellPortrait
            );

            // Add strangulation possibility clue
            _main.AddClue(
                "Strangulation Theory - Silk Scarf",
                "Eleanor's initial observation saw no blood, only Edgar slumped over. She wondered if a silk scarf was used—'like in those mystery novels.' Victorian gentlemen often carry such scarves. Other portraits later mentioned a stab wound, but Eleanor notes scarves can hide marks on the neck. Perhaps both methods were attempted? The lack of visible blood initially suggested something more subtle than a blade.",
                _eleanorHeartwellPortrait
            );

            // Unlock Silk Scarf as weapon option
            _main.UnlockOption(1, 3); // What option 4: "Silk Scarf (Strangulation)"

            // Note: Multiple Conspirators option removed (limited to 4 options per question)
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Eleanor Hartwell", _eleanorHeartwellPortrait);
        }
    }

    public void ResetState()
    {
        _visitCount = 0;
    }
}

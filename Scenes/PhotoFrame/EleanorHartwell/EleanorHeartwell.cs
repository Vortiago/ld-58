using System;
using Godot;

public partial class EleanorHeartwell : Node3D
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
    private Texture2D _eleanorHeartwellPortrait;

    // Dialog configuration - First Visit (nervous, protective, portrait perspective)
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Mrs. Hartwell, from your portrait's position, what did you observe about your husband's movements?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Inspector, my portrait faces slightly away—the artist captured me looking toward the garden. I must turn to see the hallway properly. Thomas was... he mentioned visiting the library after dinner. We portraits do enjoy our social calls to other frames.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Did you see him in the library portrait gallery?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Not directly. Thomas and I were painted together originally, but we hang in different wings now. He visits my frame often—portrait etiquette, you understand. Last night he seemed... distracted. The other portraits noticed too.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Which portraits noticed his behavior?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The Colonel in the smoking room mentioned seeing him. Or was it young Timothy's portrait? We share observations constantly. From my angle, I noticed muddy footprints near where Edgar... The servants' portraits blame the gardener, but they gossip terribly.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The gardener was working during the dinner party?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The kitchen portraits say he left at six. But footprints... they could be anyone's. Lord Blackwood himself came from the garden earlier. Even young Timothy was playing outside. My frame position makes it hard to distinguish details.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The wound angle suggests a left-handed attacker. Is Thomas left-handed?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Thomas writes with his right hand—I've watched him from my frame for years. Though the artist did capture both our hands rather oddly. Portrait artists take such liberties with anatomy.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Mrs. Hartwell, the guest register shows Thomas signed with his left hand tonight.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Did he? How peculiar. Perhaps... sometimes we portraits see what we expect to see. Or perhaps he injured his right hand at dinner? Lady Margaret's portrait mentioned something about a minor accident with the carving knife. These details blur when you exist as paint and memory.", DialogSystem.SpeakerSide.Right)
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

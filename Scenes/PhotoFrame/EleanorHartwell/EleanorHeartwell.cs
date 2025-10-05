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

    // Dialog configuration - First Visit (nervous, contradictory, trying to protect Thomas)
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Mrs. Hartwell, I need to ask about your husband's movements last night.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Oh! Inspector, yes. Thomas was in the library all evening. Reading by the fire, as he often does after dinner.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The library? Did you observe him there yourself?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Well... no, not directly. He mentioned he'd be in the smoking room, actually. That's where he usually retires. I must have confused the two—being frozen in this portrait, one loses track of such details.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("I see. From your portrait's vantage point, what can you observe of the scene below?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Those muddy footprints near the body? The gardener often tracks mud through here. He's quite dedicated, works at all hours. It must be from his evening rounds.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The gardener makes rounds during a dinner party?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Perhaps the tracks are from earlier in the day? Mud can remain wet for hours in this damp weather. Days even.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The letter opener appears to have been wielded by a left-handed person. What can you tell me about that?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Then surely that clears Thomas! My husband favors his right hand. I've watched him write hundreds of letters from this very frame.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Mrs. Hartwell, I observed your husband signing the guest register this evening. With his left hand.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I... the view from my portrait... everything appears reversed, you understand. Mirror images. It's quite disorienting, existing as paint on canvas. I may have been mistaken about which hand...", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits (still nervous, but resigned)
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Mrs. Hartwell, is there anything else you can tell me?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I've shared what I know, Inspector. From my vantage point in this frame, the details are... limited.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Sometimes the smallest details matter most.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Yes... I suppose they do. From my portrait, I watch the hallway at night. So many restless footsteps. So many secrets painted on these walls.", DialogSystem.SpeakerSide.Right)
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

        // Add clues on first visit - Eleanor's contradictions are themselves evidence
        if (_visitCount == 1)
        {
            // Add clue about Eleanor's contradictory statements
            _main.AddClue(
                "Eleanor's Contradictory Statements",
                "Mrs. Eleanor Hartwell provides conflicting accounts of Thomas's location: first the library, then corrects herself to the smoking room. She attributes her confusion to the disorienting nature of existing within a portrait. Her uncertainty about her own husband's whereabouts during the murder is notable.",
                _eleanorHeartwellPortrait
            );

            // Add clue about the muddy footprints explanation
            _main.AddClue(
                "Muddy Footprints Near Body",
                "Eleanor quickly offers an explanation for muddy tracks near the victim: 'the gardener often tracks mud through here.' When questioned about a gardener's presence during the dinner party, she suggests the mud could be from earlier in the day. Her immediate readiness to provide alternate explanations is striking.",
                _eleanorHeartwellPortrait
            );

            // Add clue about the left-handed detail
            _main.AddClue(
                "Left-Handed Discrepancy",
                "Eleanor claims Thomas 'favors his right hand,' having watched him write 'hundreds of letters' from her portrait. The Inspector observed Thomas signing the guest register with his left hand. The wound angle suggests a left-handed attacker. Eleanor's confusion about her husband's dominant hand raises questions.",
                _eleanorHeartwellPortrait
            );

            // Reinforce Thomas Hartwell as prime suspect (already unlocked by HouseKeeper)
            // Her contradictions point toward something being concealed
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Eleanor Hartwell", _eleanorHeartwellPortrait);
        }
    }
}

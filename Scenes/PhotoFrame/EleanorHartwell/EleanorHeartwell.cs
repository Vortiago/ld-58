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
    private readonly string[] _firstVisitDialog = new[]
    {
        "Mrs. Hartwell, I need to ask you about your husband's whereabouts last night.",
        "Oh! Inspector, yes, of course. Thomas was... he was in the library all evening. Reading. Yes, reading by the fire.",
        "The library? Are you certain?",
        "Well, no, wait—I meant the smoking room! Yes, the smoking room. He often retires there after dinner. My mistake, I'm simply... I'm so flustered by all this.",
        "I understand this is distressing. From your portrait's position, what can you see of the crime scene?",
        "Those muddy footprints? Oh, um... the gardener often comes through here. Yes, the gardener! He tracks mud in all the time. We've spoken to him about it repeatedly.",
        "At this hour of night? And during a murder?",
        "Well, I... perhaps it was from earlier in the day? Mud can stay wet for quite some time, can't it?",
        "Mrs. Hartwell, why are you protecting your husband?",
        "I'm not—! Thomas would never... Inspector, you must understand, he's not even left-handed! Everyone knows the killer must have been left-handed from the wound angle.",
        "Your husband IS left-handed, Mrs. Hartwell. I've seen him sign documents.",
        "Oh. I... I meant to say... What I meant was... From my portrait, I can't see everything clearly. The angle, you see. It's all very confusing from up here."
    };

    // Dialog configuration - Subsequent Visits (still nervous, but resigned)
    private readonly string[] _subsequentVisitDialog = new[]
    {
        "Mrs. Hartwell, is there anything else you'd like to tell me?",
        "I... I've told you everything I can, Inspector. Thomas is a good man. He must be.",
        "The truth will come out eventually.",
        "I know. From my portrait, I watch him pace at night, unable to sleep. I just... I hoped it wasn't true."
    };

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _eleanorHeartwellPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/EleanorHartwell/EleanorHeartwell.png");
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

        // Eleanor's nervous lying actually helps the player realize Thomas is guilty
        // She doesn't add new clues but her behavior is evidence itself
        // Her contradictions and obvious attempts to misdirect confirm suspicions

        string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            // Alternate speakers: Inspector Crawford (left) speaks first (even indices)
            bool isLeftSpeaking = true;
            _dialogSystem.StartDialog(dialogToShow, "Eleanor Hartwell", _eleanorHeartwellPortrait, isLeftSpeaking);
        }
    }
}

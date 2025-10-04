using System;
using Godot;

public partial class DrHenryMorrison : Node3D
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
    private Texture2D _drMorrisonPortrait;

    // Dialog configuration - First Visit
    private readonly string[] _firstVisitDialog = new[]
    {
        "Dr. Morrison, your medical expertise would be invaluable. What can you tell me about Lord Blackwood's death?",
        "Inspector, from my portrait's perspective, I can see the body clearly. Single stab wound, angled upward—the attacker was shorter than Edgar.",
        "Shorter? That's a significant detail. Can you determine anything else from your vantage point?",
        "The defensive wounds on his hands tell me he tried to grab the blade. He was facing his attacker, Inspector. This wasn't a surprise attack.",
        "He knew his killer, then. What about the emotional state of those involved?",
        "I've treated Thomas Hartwell for 'nervous episodes' before, Inspector. The man has a violent temper when provoked.",
        "Thomas Hartwell? That's quite an accusation, Doctor.",
        "I'm merely stating medical facts. Edgar called me yesterday, concerned about stress from 'business troubles.' He specifically mentioned Thomas.",
        "From your frame, can you see any other evidence that might support this?",
        "The wound angle and the defensive injuries are consistent with someone Edgar knew well—someone he would face directly in an argument. That's all my medical observation can tell you from here."
    };

    // Dialog configuration - Subsequent Visits
    private readonly string[] _subsequentVisitDialog = new[]
    {
        "Doctor, anything else you've observed?",
        "Only what I've already told you, Inspector. The wound pattern and Thomas's history speak volumes."
    };

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _drMorrisonPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/DrHenryMorrison/DrHenryMorrison.png");
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

        // Add defensive wounds clue on first visit
        if (_visitCount == 1)
        {
            _main.AddClue(
                "Defensive Wounds",
                "Cuts on Lord Edgar's hands and forearms show he tried to grab the blade. More wounds on the right hand - he was facing his attacker. Edgar saw his killer (not attacked from behind initially). Knew his attacker (no signs of surprise). Brief struggle before fatal blow."
            );
        }

        string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            // Alternate speakers: Inspector Crawford (left) speaks first (even indices)
            bool isLeftSpeaking = true;
            _dialogSystem.StartDialog(dialogToShow, "Dr. Henry Morrison", _drMorrisonPortrait, isLeftSpeaking);
        }
    }
}

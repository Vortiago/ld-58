using System;
using Godot;

public partial class LadyBlackwood : Node3D
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
    private Texture2D _ladyMargaretPortrait;

    // Dialog configuration - First Visit
    private readonly string[] _firstVisitDialog = new[]
    {
        "Lady Margaret, I'm deeply sorry for your loss. I know this is difficult, but I must ask what you witnessed.",
        "Inspector... from my portrait, I watched my husband die. That letter opener—Edgar's favorite, the one I gave him—used against him like that...",
        "The murder weapon was his own letter opener?",
        "Yes. It sits on his desk in the study. Only someone familiar with this house would know it was there. This wasn't some random intruder.",
        "From your frame's position, can you see any other evidence?",
        "The muddy footprints, Inspector. They're everywhere—leading from the garden entrance, through the scene, to the window. Fresh mud, still wet when it happened.",
        "Muddy footprints? That suggests someone who came in from outside.",
        "Edgar discovered something terrible about the accounts yesterday. He was reviewing the ledgers with such fury. Those financial papers—from where I hang, I can see them scattered by the corner table.",
        "Financial papers? What was in them?",
        "I don't know the details, but Edgar's face... he was devastated. Betrayed. He said someone had been stealing from the company for over a year.",
        "Did he mention who?",
        "He didn't say the name aloud, but I saw him staring at Thomas's signature on those documents. From my portrait's view, I watched his trust shatter. And now... now he's gone."
    };

    // Dialog configuration - Subsequent Visits
    private readonly string[] _subsequentVisitDialog = new[]
    {
        "Lady Margaret, is there anything else you remember?",
        "Only the pain of watching, Inspector. The muddy footprints, the letter opener, those damning financial papers... it all points to betrayal.",
        "You've been very brave. Thank you.",
        "Find who did this, Inspector. For Edgar."
    };

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _ladyMargaretPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/LadyBlackwood/LadyMargaretBlackwood.png");
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
            _main.AddClue(
                "Ornate Letter Opener (Murder Weapon)",
                "A decorative silver letter opener with an ivory handle, still embedded in the victim's back. It was kept on Lord Edgar's desk - not something a stranger would know about. The murder weapon. Taken from Edgar's own desk (killer knew the house). Not premeditated - grabbed in the heat of an argument."
            );

            _main.AddClue(
                "Muddy Footprints",
                "Muddy boot prints leading from the garden entrance, through the crime scene, to the escape window. Fresh mud - still wet. Killer entered from outside (not a dinner guest). Thomas often used the garden entrance for 'private meetings'. Tracks show someone running (wide stride)."
            );

            _main.AddClue(
                "Financial Papers (Embezzlement Evidence)",
                "Accountancy ledgers and bank statements scattered during the struggle. Red ink corrections show £50,000 missing from the company accounts over the past year. Shows the motive: Thomas stole £50,000. Edgar discovered the theft that evening. Confrontation led to murder."
            );
        }

        string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            // Alternate speakers: Inspector Crawford (left) speaks first (even indices)
            bool isLeftSpeaking = true;
            _dialogSystem.StartDialog(dialogToShow, "Lady Margaret Blackwood", _ladyMargaretPortrait, isLeftSpeaking);
        }
    }
}

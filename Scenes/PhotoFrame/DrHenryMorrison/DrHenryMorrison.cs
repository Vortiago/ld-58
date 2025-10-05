using System;
using Godot;

public partial class DrHenryMorrison : Node3D
{
    private Camera3D _camera;

    private InteractableObject _interactableObject;

    private CameraStateMonitor _cameraStateMonitor;

    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;
    private Texture2D _drMorrisonPortrait;

    // Dialog configuration - First Visit
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Dr. Morrison, your medical expertise serves you well even in painted form. What can you observe from your frame's position?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Inspector, my frame's angle gives me an excellent diagnostic view. Single puncture wound, angled upward. My medical training, preserved in oils and canvas, tells me the attacker was either shorter or crouching. We portrait doctors often consult on such matters.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Can your position reveal anything about the attacker's identity?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The defensive wounds suggest Edgar knew his attacker—he faced them directly, no surprise. From this vantage point, I can see every detail clearly. The other medical portraits and I have been discussing the wound pattern all morning.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Other medical portraits? You mean Dr. Pemberton?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("No, Victor doesn't have a portrait here yet. But the late Dr. Blackwood—Edgar's father—his portrait hangs in the library. We often compare observations. Victor was at dinner tonight, actually. Edgar threatened him over that surgery mishap.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("A serious accusation. Where was Dr. Pemberton during the murder?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The smoking room, according to the portrait of Admiral Henderson who hangs there. Though he admits he was dozing. From my position, I see surgical instruments near the body—unusual for a study.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Could those instruments be the murder weapon?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The wound suggests something thin and sharp—letter opener, scalpel, even young Timothy's fencing foil could match. Victor always carries his medical bag, true. But then, Lady Margaret mentioned Edgar kept his father's old surgical kit in the study. Multiple possibilities, Inspector.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Doctor, anything else from your medical perspective?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("My position gives me perfect view of the wounds, Inspector. The other portraits keep asking my opinion—was it a blade, was it surgical? Even we paintings have our debates. The truth remains elusive.", DialogSystem.SpeakerSide.Right)
    };

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");

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

        // Add clues and unlock options on first visit
        if (_visitCount == 1)
        {
            // Add defensive wounds clue (evidence but less conclusive)
            _main.AddClue(
                "Defensive Wounds Analysis",
                "Dr. Morrison's medical expertise reveals cuts on Edgar's hands suggesting he grabbed at the weapon. The pattern indicates Edgar faced his attacker—someone he knew. However, the portrait doctors debate whether these are defensive or offensive wounds. Could Edgar have been the initial aggressor? The wound angle suggests either a shorter attacker or someone crouching—possibly a child?",
                _drMorrisonPortrait
            );

            // Add multiple weapon possibilities clue
            _main.AddClue(
                "Weapon Possibilities",
                "Multiple sharp objects present: Dr. Pemberton's surgical scalpels (one missing), Edgar's ornate letter opener, Edgar's late father's surgical kit in the study, and young Timothy's fencing foil mentioned by portraits. The wound could match any thin, sharp blade. Morrison notes the precision suggests either medical knowledge or lucky strike.",
                _drMorrisonPortrait
            );

            // Add portrait testimony conflict
            _main.AddClue(
                "Conflicting Medical Opinions",
                "Dr. Morrison consults with other medical portraits including Edgar's late father. They disagree on the wound's nature—surgical precision or amateur's luck? Admiral Henderson's portrait claims Pemberton was in the smoking room but admits he was 'dozing.' Portrait testimony is only as reliable as the portrait's attention span.",
                _drMorrisonPortrait
            );

            // Add conspiracy hint
            _main.AddClue(
                "Attacker's Height Analysis",
                "The upward wound angle indicates someone shorter than Edgar or attacking from below. Could be: a woman (Lady Margaret), a younger person (Timothy), someone crouching (anyone), or someone who fell during struggle. Dr. Morrison's portrait colleagues suggest multiple interpretations.",
                _drMorrisonPortrait
            );

            // Unlock Dr. Pemberton as suspect
            _main.SetOptionText(0, 1, "Dr. Victor Pemberton"); // Who option 2
            _main.UnlockOption(0, 1);

            // Unlock multiple weapon options
            _main.SetOptionText(1, 0, "Ornate Letter Opener"); // What option 1
            _main.UnlockOption(1, 0);

            // Note: Medical Scalpel option removed (limited to 4 options per question)

            // Unlock medical cover-up motive
            _main.SetOptionText(2, 3, "Medical Cover-up"); // Why option 4
            _main.UnlockOption(2, 3);
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Dr. Henry Morrison", _drMorrisonPortrait);
        }
    }

    public void ResetState()
    {
        _visitCount = 0;
    }
}

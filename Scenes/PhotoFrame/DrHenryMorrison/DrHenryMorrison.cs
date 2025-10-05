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
        new DialogSystem.DialogLine("Dr. Morrison, what can you observe from your frame's position?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Single puncture wound, angled upward. Defensive wounds on his hands—Edgar knew his attacker and faced them directly.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What else can you see from there?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Surgical instruments near the body—unusual. Dr. Pemberton was at dinner. Edgar had threatened him over a botched surgery.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The wine glass near the body—could poison be involved?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I examined the residue. Digitalis traces are present.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Poison and stabbing?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Perhaps poison was attempted first, then panic led to the blade. Or Edgar was simply drinking during the confrontation. Hard to say.", DialogSystem.SpeakerSide.Right)
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

            // Add poison possibility clue
            _main.AddClue(
                "Poison Theory - Digitalis",
                "Dr. Morrison examined the wine glass residue and found traces consistent with digitalis poisoning. He suggests poison may have been attempted initially—the killer perhaps panicked when it didn't work fast enough and grabbed the letter opener. The stab wound appears fatal, but was Edgar already dying from poison? The wine glass was knocked over during the struggle, complicating the evidence.",
                _drMorrisonPortrait
            );

            // Unlock Dr. Pemberton as suspect
            _main.SetOptionText(0, 1, "Dr. Victor Pemberton"); // Who option 2
            _main.UnlockOption(0, 1);

            // Unlock Poison as weapon option
            _main.UnlockOption(1, 1); // What option 2: "Poison (Digitalis)"

            // Note: Ornate Letter Opener now unlocked by examining it directly in the hallway

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

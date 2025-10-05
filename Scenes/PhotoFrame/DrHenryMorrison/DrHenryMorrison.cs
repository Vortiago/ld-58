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
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Dr. Morrison, your medical expertise would be invaluable. From your portrait, what can you observe about Lord Blackwood's death?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Inspector, I've been watching the scene unfold from my frame. Single stab wound, angled upward. The attacker was shorter than Edgar, or struck from below. Living within this portrait gives one an excellent vantage point for observation.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("From your perspective in the painting, can you determine anything else?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The defensive wounds on his hands—he tried to grab the blade. We portrait dwellers witness everything from our frames, Inspector. He faced his attacker directly. It's quite remarkable what one notices when observing from painted canvas.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Speaking of medical matters, weren't you and Dr. Pemberton both dinner guests tonight?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Indeed, Victor Pemberton attended the dinner. I overheard Edgar earlier—he was furious about a botched surgery Victor performed. Threatened to report him to the medical board. We portraits overhear everything that happens in these halls.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("That's a strong motive. Where was Dr. Pemberton when the murder occurred?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The smoking room, I believe. From where I reside in my frame, I cannot see around corners, unfortunately. What I can observe: surgical instruments near the body. Quite unusual to find in a study.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Surgical instruments? At a murder scene?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Victor always carries his medical bag—professional habit. If he was desperate enough... The wound is precise, Inspector. Almost surgical in nature. Living here in this frame, I've had ample time to study every detail of the scene.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Doctor, anything else you've observed from your portrait?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Only what I've already shared, Inspector. The defensive wounds, the surgical instruments. Living in this frame, I can see the scene quite clearly, but the evidence speaks for itself.", DialogSystem.SpeakerSide.Right)
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

        // Add clues and unlock options on first visit
        if (_visitCount == 1)
        {
            // Add defensive wounds clue (real evidence)
            _main.AddClue(
                "Defensive Wounds",
                "Cuts on Lord Edgar's hands and forearms show he tried to grab the blade. More wounds on the right hand - he was facing his attacker. Edgar saw his killer (not attacked from behind initially). Knew his attacker (no signs of surprise). Brief struggle before fatal blow.",
                _drMorrisonPortrait
            );

            // Add red herring clue about Dr. Pemberton
            _main.AddClue(
                "Surgical Instruments (Dr. Pemberton)",
                "A leather medical bag with surgical scalpels visible near the body. Dr. Pemberton always carries his instruments - professional habit. Unusual placement suggests bag was set down hastily. One scalpel is missing from the set. Could indicate medical knowledge in the killer. However, multiple witnesses placed Dr. Pemberton in the smoking room during the murder.",
                _drMorrisonPortrait
            );

            // Add red herring clue about Dr. Pemberton's motive
            _main.AddClue(
                "Dr. Pemberton's Motive",
                "Edgar threatened to report Dr. Pemberton to the medical board for a botched surgery last month. Dr. Morrison mentions Victor was 'desperate enough' and the wound is 'almost surgical in execution.' Strong motive for medical cover-up murder.",
                _drMorrisonPortrait
            );

            // Unlock Dr. Pemberton as red herring suspect
            _main.SetOptionText(0, 1, "Dr. Victor Pemberton"); // Who option 2
            _main.UnlockOption(0, 1);

            // Unlock letter opener as weapon (mentioned in dialog as precise wound)
            _main.SetOptionText(1, 0, "Ornate Letter Opener"); // What option 1
            _main.UnlockOption(1, 0);

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
}

using System;
using Godot;

public partial class YoungTimBlackwood : Node3D
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
    private Texture2D _youngTimothyPortrait;

    // Dialog configuration - First Visit
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Hello there, young man. You're Timothy, aren't you? Lord Blackwood's son?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Yes, sir. I'm... I'm not supposed to be up this late. Am I in trouble?", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("No, no trouble at all. But from your portrait here, you might have seen something important. Can you tell me what happened?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I wasn't supposed to be up, but I heard shouting... Papa's voice and someone else. They sounded really angry. I got scared and hid in my portrait.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Someone else? Could you tell who it was from the voice?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("It... it sounded like a man. But I'm not sure who. Miss Catherine was reading me stories earlier, then she left to check on something downstairs.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Miss Catherine, your governess? She was with you before this happened?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Yes, until about... I don't know the time exactly. She seemed nervous, kept looking at the clock. Then someone ran past my portrait—I saw them from my frame, but it was too dark to see their face clearly.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What did you notice about the person who ran past?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("They were breathing really hard and moving fast. Earlier that evening, I saw Papa looking at papers on his desk. The ones with all the numbers. He looked so sad, like when our dog died.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("The financial papers? Did your father say anything about them?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("He kept saying 'How could they do this?' over and over. 'They', not 'he' or 'she'. I remember because it confused me—who was he talking about?", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Timothy, is there anything else you remember from that night?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Just someone running away in the dark. I wish I could have seen their face, Inspector. And Miss Catherine acting so nervous before she left me...", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("You're very brave for telling me. Thank you.", DialogSystem.SpeakerSide.Left)
    };

    public override void _Ready()
    {
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
            // Add clue about Miss Catherine's nervous behavior (red herring reinforcement)
            _main.AddClue(
                "Miss Catherine's Nervous Behavior",
                "Timothy reports Miss Catherine seemed nervous while reading stories, kept looking at the clock. She left him suddenly to 'check on something downstairs' shortly before the murder. Her unusual behavior and timing make her a suspect. However, Timothy confirms she was with him reading during the critical time window.",
                _youngTimothyPortrait
            );

            // Add clue about Edgar's "they" statement (ambiguous evidence)
            _main.AddClue(
                "Edgar's 'They' Statement",
                "Timothy heard his father say 'How could they do this?' while looking at financial papers. Edgar used 'they' (plural) not 'he' or 'she'. Could indicate multiple people involved in the embezzlement, or Edgar was speaking generally about betrayal. Creates ambiguity about number of suspects.",
                _youngTimothyPortrait
            );

            // Add clue about unidentified runner (ambiguous witness account)
            _main.AddClue(
                "Unidentified Runner",
                "Timothy saw someone running past his portrait frame after the murder, breathing hard and moving fast. Too dark to identify face or distinguishing features. Could have been any of the dinner guests fleeing the scene. Confirms someone fled but doesn't narrow suspects.",
                _youngTimothyPortrait
            );

            // Reinforce Miss Catherine as suspect (she's already unlocked by Lady Blackwood, but this provides her alibi)
            // The clue itself mentions she was WITH Timothy, which actually gives her an alibi
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Young Timothy Blackwood", _youngTimothyPortrait);
        }
    }
}

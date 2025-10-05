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
        new DialogSystem.DialogLine("Hello there, young Timothy. Your portrait is quite new, isn't it?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Yes sir, my painting still smells of fresh oils. The older portraits tell me I'll get used to living in a frame. The portrait children in the nursery wing are teaching me how to visit other frames.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("You can visit other frames? Where were you when this happened to your father?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I was in my own frame, I promise! Though earlier I did sneak to the library portraits—they tell the best stories about the house. Papa was looking at papers there, the ones with numbers. He looked so upset.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What did you observe from your frame position during the incident?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I heard shouting—Papa and someone else. The older portraits say I'm too young to understand, but I know angry voices. Then someone ran past my frame, breathing hard. My position let me see their shadow but not their face.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Tell me about Miss Catherine. Where was she?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("She was reading to me earlier—portrait etiquette, she calls it. She visits my frame every night. But she seemed different, kept checking something. She said she had to 'take care of something' and left. The other portraits whisper that she protects me, but from what?", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Protects you? What do you mean?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I don't know exactly. Sometimes when I practice moving between frames, I end up places I shouldn't. Last week I was in Papa's study frame and saw... things. Miss Catherine found me and brought me back. She said some frames aren't safe for young portraits.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What did you see in your father's study frame?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Papers about me, I think. And about Uncle Timothy Hartwell—that's who I'm named after. He died before I was born, but his portrait used to hang here. Papa had it removed last month. The older portraits won't tell me why. They just say 'family secrets are painted in layers.'", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Your father said 'How could THEY do this?' Who do you think 'they' were?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I don't know, sir. But before dinner, I heard Papa arguing with Lady Margaret's portrait. Can portraits argue with living people? The older paintings say we can influence things, but they won't teach me how yet. They say I'm too young, that I might do something... wrong.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Timothy, is there anything else you remember?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The portrait children keep asking if I saw who did it. I tell them about the shadow, but... Inspector, what if the shadow looked familiar? What if it was someone I know? Even portraits can have nightmares.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Who do you think it might have been?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("I... I shouldn't guess. Miss Catherine says guessing can hurt people. But the handkerchief everyone talks about—'T.H.'—those are my initials too. Timothy Hartwell. Just like my dead uncle.", DialogSystem.SpeakerSide.Right)
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
            // Add clue about Timothy's frame abilities
            _main.AddClue(
                "Young Portrait's Abilities",
                "Timothy can move between frames—unusual for such a new portrait. He was found in his father's study frame last week seeing 'things' before Miss Catherine brought him back. The older portraits say he might 'do something wrong' if taught too much. Portrait children are teaching him frame travel. Where was he really during the murder?",
                _youngTimothyPortrait
            );

            // Add clue about the removed portrait and family secret
            _main.AddClue(
                "Uncle Timothy Hartwell's Portrait",
                "Young Timothy is named after his uncle Timothy Hartwell who 'died before he was born.' Uncle's portrait was removed last month by Edgar. The handkerchief's 'T.H.' monogram matches both Timothys. Older portraits say 'family secrets are painted in layers.' What happened to the first Timothy Hartwell?",
                _youngTimothyPortrait
            );

            // Add clue about Miss Catherine's protection
            _main.AddClue(
                "Catherine's Mysterious Protection",
                "Miss Catherine visits Timothy's frame nightly for 'portrait etiquette.' Other portraits whisper she 'protects' him but won't say from what. She prevents him from visiting certain frames calling them 'unsafe for young portraits.' She left to 'take care of something' before the murder. Is she protecting Timothy or covering for him?",
                _youngTimothyPortrait
            );

            // Add clue about portrait influence
            _main.AddClue(
                "Portrait-Human Interaction",
                "Timothy heard Edgar arguing with Lady Margaret's portrait before dinner. Can portraits influence living people? Older paintings say they can but won't teach Timothy yet—he's 'too young' and might do something 'wrong.' This suggests portraits may have more agency than mere observers.",
                _youngTimothyPortrait
            );

            // Add clue about the familiar shadow
            _main.AddClue(
                "The Familiar Shadow",
                "Timothy saw a shadow running past but claims he couldn't identify it. On repeat visits, he admits: 'What if the shadow looked familiar?' He knows 'T.H.' are his initials too. His reluctance to identify the runner combined with Miss Catherine's protection suggests he recognized someone—or was involved himself.",
                _youngTimothyPortrait
            );

            // Note: Additional motive options removed (limited to 4 options per question)
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Young Timothy Blackwood", _youngTimothyPortrait);
        }
    }

    public void ResetState()
    {
        _visitCount = 0;
    }
}

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
    private readonly string[] _firstVisitDialog = new[]
    {
        "Hello there, young man. You're Timothy, aren't you? Lord Blackwood's son?",
        "Yes, sir. I'm... I'm not supposed to be up this late. Am I in trouble?",
        "No, no trouble at all. But from your portrait here, you might have seen something important. Can you tell me what happened?",
        "I wasn't supposed to be up, but I heard shouting... Papa's voice and Uncle Thomas. They sounded really angry.",
        "Uncle Thomas? You mean Thomas Hartwell?",
        "He's not really my uncle, but Papa always told me to call him that. He's Papa's business partner. Was Papa's business partner...",
        "I'm sorry, Timothy. What did you see from your frame?",
        "Uncle Thomas ran past me, right below where my portrait hangs. He looked scared, Inspector. His eyes were all wide and he was breathing hard.",
        "That must have been frightening. Did you notice anything else about him?",
        "His hands... I remember his hands were shaking. And earlier that evening, before bed, I saw Papa looking at those papers on his desk. The ones with all the numbers.",
        "The financial papers? Did your father say anything about them?",
        "He looked sad. Really, really sad. Like when our dog died last year. He kept saying 'How could he do this?' over and over."
    };

    // Dialog configuration - Subsequent Visits
    private readonly string[] _subsequentVisitDialog = new[]
    {
        "Timothy, is there anything else you remember from that night?",
        "Just Uncle Thomas running away. He looked so scared, Inspector. I've never seen a grown-up look that scared before.",
        "You're very brave for telling me. Thank you."
    };

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _youngTimothyPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/YoungTimBlackwood/YoungTimothyBlackwood.png");
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

        // Timothy's testimony corroborates the financial motive but doesn't add new physical clues
        // His innocent observation reinforces existing evidence rather than introducing new items

        string[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            // Alternate speakers: Inspector Crawford (left) speaks first (even indices)
            bool isLeftSpeaking = true;
            _dialogSystem.StartDialog(dialogToShow, "Young Timothy Blackwood", _youngTimothyPortrait, isLeftSpeaking);
        }
    }
}

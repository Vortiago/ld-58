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
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Lady Margaret, I'm deeply sorry. Your portrait hangs directly across from where Edgar fell. What did you observe?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Inspector, my portrait was commissioned just last year—I'm still learning from the older portraits here. My frame gives me perfect view of... of where Edgar died. Being a newer portrait, the household secrets are still revealing themselves to me.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What secrets have the other portraits shared with you?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Miss Catherine Ashworth, the governess—the servants' portraits whisper about her gambling debts. £5,000 to dangerous people. Edgar planned to dismiss her. Though the nursery portraits say she's devoted to young Timothy, almost protectively so.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Protective of Timothy? In what way?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The older portraits won't say directly. Portrait etiquette, you understand. But Edgar discovered something yesterday—financial irregularities. From my position, I watched him review the ledgers. Such anger in his face.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Who had access to manipulate the company finances?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Thomas Hartwell, the business partner. James Whitmore, our lawyer. Even I have signing authority—Edgar insisted when we married. The portrait of Edgar's father warned him about giving too many people access, but Edgar trusted those close to him.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("You have signing authority? That's unusual for the era.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Edgar was progressive in some ways. From my frame, I saw him write something before dinner—a new will, perhaps? He burned it in the fireplace afterward. The other portraits were whispering about it all evening. Even we paintings have our gossip networks, Inspector.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("A new will? Do you know what it contained?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The portrait of Edgar's solicitor in the study might know, but he's been strangely quiet since last night. Some portraits choose silence, Inspector. We all have our reasons for what we reveal and what we conceal.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Lady Margaret, is there anything else you've remembered?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("My frame watches over Edgar's body even now, Inspector. The older portraits keep asking me what I saw, but some things... some things are too painful even for a painting to relive.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Even the smallest detail could help.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The other portraits say I was restless all evening, moving within my frame more than usual. A newer portrait shouldn't be able to do that yet. Perhaps grief teaches us abilities we didn't know we had.", DialogSystem.SpeakerSide.Right)
    };

    public override void _Ready()
    {
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
        _ladyMargaretPortrait = GD.Load<Texture2D>("res://Scenes/PhotoFrame/LadyBlackwood/Lady Margaret Blackwood.png");
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
            // Add financial access clue showing multiple people could be guilty
            _main.AddClue(
                "Multiple Financial Signatures",
                "Lady Margaret reveals she has signing authority—unusual for the era but Edgar was 'progressive.' Financial documents show signatures from: Thomas Hartwell (partner), James Whitmore (lawyer), Lady Margaret herself, and even some in Edgar's name that look different. £50,000 missing over the past year. Multiple people had means and opportunity.",
                _ladyMargaretPortrait
            );

            // Add burned will clue suggesting inheritance motive
            _main.AddClue(
                "The Burned Will",
                "Lady Margaret observed Edgar writing something before dinner—possibly a new will—which he then burned in the fireplace. The portrait of Edgar's solicitor 'has been strangely quiet since last night.' What changes did the will contain? Who benefited from its destruction? The current will leaves everything to Lady Margaret.",
                _ladyMargaretPortrait
            );

            // Add Miss Catherine protection clue hinting at Timothy
            _main.AddClue(
                "Miss Catherine's Protection",
                "Servants' portraits report Catherine's gambling debts (£5,000), but nursery portraits say she's 'devoted to young Timothy, almost protectively so.' The older portraits won't explain what needs protecting. Is she covering for Timothy? Or is Timothy's secret what Edgar discovered?",
                _ladyMargaretPortrait
            );

            // Add Lady Margaret's unusual abilities
            _main.AddClue(
                "Restless Portrait Behavior",
                "Other portraits report Lady Margaret was 'restless all evening, moving within her frame more than usual.' She admits: 'A newer portrait shouldn't be able to do that yet.' Her frame position gives perfect view of the murder scene. Was she just observing, or could she somehow influence events? Portrait abilities remain mysterious.",
                _ladyMargaretPortrait
            );

            // Add portrait silence conspiracy clue
            _main.AddClue(
                "Portrait Code of Silence",
                "Lady Margaret reveals: 'Some portraits choose silence. We all have our reasons for what we reveal and conceal.' Multiple portraits are withholding information—the solicitor's portrait, older family portraits. Are they protecting someone? Or complicit in something larger?",
                _ladyMargaretPortrait
            );

            // Unlock Miss Catherine as suspect
            _main.SetOptionText(0, 3, "Miss Catherine Ashworth"); // Who option 4
            _main.UnlockOption(0, 3);

            // Unlock embezzlement motive
            _main.SetOptionText(2, 1, "Business Fraud/Embezzlement"); // Why option 2
            _main.UnlockOption(2, 1);

            // Note: Additional motive options removed (limited to 4 options per question)
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Lady Margaret Blackwood", _ladyMargaretPortrait);
        }
    }

    public void ResetState()
    {
        _visitCount = 0;
    }
}

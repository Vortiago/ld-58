using System;
using Godot;

public partial class LadyBlackwood : Node3D
{
    private Camera3D _camera;

    private InteractableObject _interactableObject;

    private CameraStateMonitor _cameraStateMonitor;

    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;
    private Texture2D _ladyMargaretPortrait;

    // Dialog configuration - First Visit
    private readonly DialogSystem.DialogLine[] _firstVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Lady Margaret, I'm deeply sorry. What did you observe?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("My portrait has perfect view of where Edgar died. Miss Catherine, the governess—she has gambling debts. £5,000. Edgar planned to dismiss her.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What else did you see?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Edgar discovered financial irregularities yesterday. Multiple people had access to the company accounts—Thomas Hartwell, James Whitmore, even myself.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("From your position, what else can you observe about the scene?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The heavy brass candlestick on the corner table. Solid, weighty.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Could that be the murder weapon?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("A single blow could have killed him. Blood is hard to see on brass from here. The letter opener was there too—both within reach.", DialogSystem.SpeakerSide.Right)
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
        _camera = GetNode<Camera3D>("Camera3D");
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _cameraStateMonitor = GetNode<CameraStateMonitor>("Camera3D/CameraStateMonitor");

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

            // Add candlestick weapon possibility clue
            _main.AddClue(
                "Heavy Brass Candlestick",
                "Lady Margaret's frame provides perfect view of the corner table's heavy brass candlestick. She suspected it as the murder weapon—'A single blow could have killed him.' The portraits examined it closely, though blood can be difficult to see on certain metals from a painted perspective. The candlestick was within arm's reach during the struggle. Did someone grab it, or reach past it for the letter opener?",
                _ladyMargaretPortrait
            );

            // Unlock Miss Catherine as suspect
            _main.SetOptionText(0, 3, "Miss Catherine Ashworth"); // Who option 4
            _main.UnlockOption(0, 3);

            // Unlock Candlestick as weapon option
            _main.UnlockOption(1, 2); // What option 3: "Heavy Candlestick"

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

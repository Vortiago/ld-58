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
        new DialogSystem.DialogLine("Lady Margaret, I'm deeply sorry for your loss. I know this is difficult, but I must ask what you witnessed.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Inspector... from my portrait, I watched my husband die. So many people had reasons to want him dead. Our dinner party guests... any of them could have...", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("What do you mean? Who specifically had grievances with your husband?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("That governess, Miss Catherine Ashworth—Edgar discovered her gambling debts. £5,000 owed to dangerous people. He was planning to dismiss her without a reference.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("That's a serious financial motive. What else did you observe?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Edgar discovered something terrible about the accounts yesterday. He was reviewing ledgers with such fury. Those financial papers—from where I hang, I can see them scattered by the corner table.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Financial papers? What was in them?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Someone had been stealing from the company for over a year. Edgar was devastated—betrayed by someone he trusted. But he didn't tell me who before... before this happened.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("Did anyone have access to those documents?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Our lawyer, James Whitmore, was here for dinner. He handles all the company finances. Edgar had been questioning some of his recent transactions... Oh Inspector, there were so many secrets in this house.", DialogSystem.SpeakerSide.Right)
    };

    // Dialog configuration - Subsequent Visits
    private readonly DialogSystem.DialogLine[] _subsequentVisitDialog = new[]
    {
        new DialogSystem.DialogLine("Lady Margaret, is there anything else you remember?", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Only the pain of watching, Inspector. Those financial papers scattered everywhere... someone Edgar trusted betrayed him terribly.", DialogSystem.SpeakerSide.Right),
        new DialogSystem.DialogLine("You've been very brave. Thank you.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Find who did this, Inspector. For Edgar.", DialogSystem.SpeakerSide.Right)
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
            // Add financial embezzlement clue (real evidence pointing to Thomas)
            _main.AddClue(
                "Financial Papers (Embezzlement Evidence)",
                "Accountancy ledgers and bank statements scattered during the struggle. Red ink corrections show £50,000 missing from the company accounts over the past year. Multiple signatures on the documents - business partner, lawyer, and accountant all had access. Edgar discovered the theft that evening. Confrontation led to murder.",
                _ladyMargaretPortrait
            );

            // Add red herring clue about Miss Catherine's gambling debts
            _main.AddClue(
                "Gambling Debt Notice (Miss Catherine)",
                "A crumpled letter partially hidden under the overturned table, addressed to Miss Catherine Ashworth. Demands immediate payment of £5,000 to 'The Golden Lion Club'. Threatens 'severe consequences' if not paid by month's end. Edgar was planning to dismiss her without a reference. Shows desperate financial motive.",
                _ladyMargaretPortrait
            );

            // Add red herring clue about James Whitmore
            _main.AddClue(
                "Lawyer's Financial Access (Whitmore)",
                "James Whitmore, the family lawyer, handles all company finances. Edgar had been questioning some of his recent transactions. Whitmore was at the dinner party and had access to the financial documents. However, multiple witnesses confirm he left at 10:45 PM before the murder.",
                _ladyMargaretPortrait
            );

            // Unlock Miss Catherine as red herring suspect
            _main.SetOptionText(0, 3, "Miss Catherine Ashworth"); // Who option 4
            _main.UnlockOption(0, 3);

            // Unlock embezzlement motive (correct answer)
            _main.SetOptionText(2, 1, "Business Fraud/Embezzlement"); // Why option 2
            _main.UnlockOption(2, 1);

            // Unlock blackmail motive (red herring based on Miss Catherine)
            _main.SetOptionText(2, 2, "Blackmail Secret"); // Why option 3
            _main.UnlockOption(2, 2);
        }

        DialogSystem.DialogLine[] dialogToShow = _visitCount == 1 ? _firstVisitDialog : _subsequentVisitDialog;

        if (dialogToShow != null && dialogToShow.Length > 0)
        {
            _dialogSystem.StartDialog(dialogToShow, "Lady Margaret Blackwood", _ladyMargaretPortrait);
        }
    }
}

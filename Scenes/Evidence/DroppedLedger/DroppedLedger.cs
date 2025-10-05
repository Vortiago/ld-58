using System;
using Godot;

public partial class DroppedLedger : Node3D
{
    private InteractableObject _interactableObject;
    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;

    private readonly DialogSystem.DialogLine[] _examinationDialog = new[]
    {
        new DialogSystem.DialogLine("A company accounts ledger. Dropped here during someone's hasty escape.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The pages show false entries... systematic theft over months. This is clear evidence of embezzlement.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The handwriting matches the documents in Edgar's study. Someone was stealing from the business and got caught.", DialogSystem.SpeakerSide.Left)
    };

    public override void _Ready()
    {
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;

        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
        _main = GetNode<Main>("/root/Main");
    }

    public override void _ExitTree()
    {
        _interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
    }

    private void OnInteractableObjectClicked()
    {
        _visitCount++;

        // Add clue on first visit
        if (_visitCount == 1)
        {
            _main.AddClue(
                "Financial Evidence",
                "Company accounts ledger dropped during Thomas's escape. Pages show false entries in Thomas's handwriting - systematic theft over months totaling £50,000. The killer was carrying this ledger, likely trying to destroy evidence when confronted by Lord Edgar."
            );

            // Unlock embezzlement motive option
            _main.SetOptionText(2, 1, "Business Fraud/Embezzlement");
            _main.UnlockOption(2, 1);
        }

        // Show examination dialog (Inspector's internal monologue)
        _dialogSystem.StartDialog(_examinationDialog, null, null);
    }
}

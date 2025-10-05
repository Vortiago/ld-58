using System;
using Godot;

public partial class Handkerchief : Node3D
{
    private InteractableObject _interactableObject;
    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;

    private readonly DialogSystem.DialogLine[] _examinationDialog = new[]
    {
        new DialogSystem.DialogLine("An expensive silk handkerchief with the monogram 'T.H.' embroidered in gold thread.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("There are dark stains on it... blood. Dropped near the plant stand during the escape.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The initials 'T.H.'... someone wouldn't leave such an expensive item behind unless they were panicked.", DialogSystem.SpeakerSide.Left)
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
                "T.H. Monogrammed Handkerchief",
                "An expensive silk handkerchief with blood stains and the monogram 'T.H.' in gold thread. Dropped near the potted plant in the escape route. The owner's initials... but who? An expensive item someone wouldn't normally leave behind, showing their panic during escape."
            );

            // Unlock Thomas Hartwell as suspect
            _main.SetOptionText(0, 2, "Thomas Hartwell");
            _main.UnlockOption(0, 2);
        }

        // Show examination dialog (Inspector's internal monologue)
        _dialogSystem.StartDialog(_examinationDialog, null, null);
    }
}

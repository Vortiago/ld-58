using System;
using Godot;

public partial class LetterOpener : Node3D
{
    private InteractableObject _interactableObject;
    private DialogSystem _dialogSystem;
    private Main _main;
    private int _visitCount = 0;

    private readonly DialogSystem.DialogLine[] _examinationDialog = new[]
    {
        new DialogSystem.DialogLine("A decorative silver letter opener with an ivory handle. Blood stains along the blade.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("Curious... this was kept on Lord Edgar's desk. Not something a stranger would know about.", DialogSystem.SpeakerSide.Left),
        new DialogSystem.DialogLine("The killer grabbed it in the heat of the moment. This wasn't premeditated murder.", DialogSystem.SpeakerSide.Left)
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
                "Possible Murder Weapon",
                "A decorative silver letter opener with an ivory handle. Blood stains along the blade. It was kept on Lord Edgar's desk - not something a stranger would know about. The killer grabbed it in the heat of the moment, suggesting this wasn't premeditated murder."
            );

            // Unlock letter opener as weapon option
            _main.SetOptionText(1, 0, "Ornate Letter Opener");
            _main.UnlockOption(1, 0);
        }

        // Show examination dialog (Inspector's internal monologue)
        _dialogSystem.StartDialog(_examinationDialog, null, null);
    }
}

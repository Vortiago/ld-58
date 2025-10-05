using System;
using Godot;

public partial class LetterOpener : Node3D
{
    [Node("InteractableObject")]
    private InteractableObject _interactableObject;

    public override void _Ready()
    {
        _interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
    }

    public override void _ExitTree()
    {
        _interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
    }

    private void OnInteractableObjectClicked()
    {
        GD.Print("Letter opener clicked");
    }
}

using System;
using Godot;

public partial class Handkerchief : Node3D
{

    private InteractableObject _interactableObject;

    public override void _Ready()
    {
        _interactableObject = GetNode<InteractableObject>("InteractableObject");
        _interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
    }

    public override void _ExitTree()
    {
        _interactableObject.InteractableObjectClicked -= OnInteractableObjectClicked;
    }

    private void OnInteractableObjectClicked()
    {
        GD.Print("Handkerchief clicked");
    }
}

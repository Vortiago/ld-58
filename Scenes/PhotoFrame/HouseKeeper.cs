using System;
using Godot;

public partial class HouseKeeper : Node3D
{
    [Node("Camera3D")]
    private Camera3D _camera;

    [Node("InteractableObject")]
    private InteractableObject _interactableObject;

    public override void _Ready()
    {
        _interactableObject.InteractableObjectClicked += OnInteractableObjectClicked;
    }

    private void OnInteractableObjectClicked()
    {
        _camera.MakeCurrent();
    }
}

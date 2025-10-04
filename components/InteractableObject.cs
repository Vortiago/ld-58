using System;
using Godot;

public partial class InteractableObject : Area3D
{
    [Signal] public delegate void InteractableObjectClickedEventHandler();

    [Export]
    public MeshInstance3D Model { get; set; }

    [Export]
    public ShaderMaterial HighlightMaterial { get; set; }



    public override void _Ready()
    {
        this.MouseEntered += OnMouseEntered;
        this.MouseExited += OnMouseExited;
        this.InputEvent += OnInputEvent;
    }

    public override void _ExitTree()
    {
        this.MouseEntered -= OnMouseEntered;
        this.MouseExited -= OnMouseExited;
        this.InputEvent -= OnInputEvent;
    }

    private void OnInputEvent(Node camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(SignalName.InteractableObjectClicked);
        }
    }

    private void OnMouseEntered()
    {
        GD.Print($"{Name} is hovered over.");
        ToggleHighlight(true);
    }

    private void OnMouseExited()
    {
        GD.Print($"{Name} is no longer hovered over.");
        ToggleHighlight(false);
    }

    private void ToggleHighlight(bool highlight)
    {
        if (Model == null || HighlightMaterial == null)
        {
            GD.PrintErr("Model or HighlightMaterial is not set.");
            return;
        }

        if (highlight)
        {
            Model.MaterialOverlay = HighlightMaterial;
        }
        else
        {
            Model.MaterialOverlay = null;
        }
    }
}

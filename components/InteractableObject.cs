using System;
using Godot;

public partial class InteractableObject : Area3D
{
    [Signal] public delegate void InteractableObjectClickedEventHandler();

    [Export]
    public MeshInstance3D Model { get; set; }

    [Export]
    public ShaderMaterial HighlightMaterial { get; set; }

    private DialogSystem _dialogSystem;



    public override void _Ready()
    {
        this.MouseEntered += OnMouseEntered;
        this.MouseExited += OnMouseExited;
        this.InputEvent += OnInputEvent;

        // Get DialogSystem reference (assuming it's in Main scene)
        _dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
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
            // Block clicks if dialog is active
            if (_dialogSystem != null && _dialogSystem.IsDialogActive)
            {
                GD.Print($"Click blocked on {Name} - dialog is active");
                return;
            }

            EmitSignal(SignalName.InteractableObjectClicked);
        }
    }

    private void OnMouseEntered()
    {
        GD.Print($"{GetParent().Name}/{Name} is hovered over.");
        ToggleHighlight(true);
    }

    private void OnMouseExited()
    {
        GD.Print($"{GetParent().Name}/{Name} is no longer hovered over.");
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

    /// <summary>
    /// Enables or disables input ray picking for this interactable object.
    /// When disabled, the object cannot be clicked or hovered.
    /// </summary>
    public void SetCollisionEnabled(bool enabled)
    {
        InputRayPickable = enabled;
        GD.Print($"{GetParent().Name}/{Name} InputRayPickable set to {enabled}");
    }
}

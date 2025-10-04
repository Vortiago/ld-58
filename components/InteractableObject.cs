using Godot;
using System;

public partial class InteractableObject : Area3D
{
    private StandardMaterial3D[] modelMaterials;

    [Export]
    public MeshInstance3D Model { get; set; }

    [Export]
    public ShaderMaterial HighlightMaterial { get; set; }

    public override void _Ready()
    {
        var materialCount = Model.GetSurfaceOverrideMaterialCount();
        modelMaterials = new StandardMaterial3D[materialCount];
        for (int i = 0; i < materialCount; i++)
        {
            modelMaterials[i] = (StandardMaterial3D)Model.GetSurfaceOverrideMaterial(i);
        }

        this.MouseEntered += OnMouseEntered;
        this.MouseExited += OnMouseExited;
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
        for (int i = 0; i < modelMaterials.Length; i++)
        {
            Model.SetSurfaceOverrideMaterial(i, highlight ? HighlightMaterial : modelMaterials[i]);
        }
    }
}

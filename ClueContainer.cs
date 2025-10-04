using System;
using Godot;

public partial class ClueContainer : PanelContainer
{
    PackedScene ClueItemScene = GD.Load<PackedScene>("res://ClueItem.tscn");

    [Export] private Button CloseButton;

    [Node("%ClueGrid")]
    private GridContainer ClueItemsGrid;

    public override void _Ready()
    {
        CloseButton.Pressed += OnCloseButtonPressed;
        Visible = false;
    }

    public override void _ExitTree()
    {
        CloseButton.Pressed -= OnCloseButtonPressed;
    }

    public void CreateClueItem(string header, string body)
    {
        ClueItem clueItem = (ClueItem)ClueItemScene.Instantiate();
        clueItem.ClueHeader = header;
        clueItem.ClueBody = body;
        ClueItemsGrid.AddChild(clueItem);
    }

    public new void Show()
    {
        Visible = true;
    }

    public new void Hide()
    {
        Visible = false;
    }

    private void OnCloseButtonPressed()
    {
        Hide();
    }
}

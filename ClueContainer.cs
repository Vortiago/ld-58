using System;
using Godot;

public partial class ClueContainer : PanelContainer
{
    PackedScene ClueItemScene = GD.Load<PackedScene>("res://ClueItem.tscn");

    [Node("%ClueGrid")]
    private GridContainer ClueItemsGrid;

    public override void _Ready()
    {
        // Optionally, you can initialize or load clues here

        // Example of adding a clue item
        for (int i = 1; i <= 5; i++)
        {
            CreateClueItem($"Clue {i}", $"This is the description for clue {i}.");
        }
    }

    public void CreateClueItem(string header, string body)
    {
        ClueItem clueItem = (ClueItem)ClueItemScene.Instantiate();
        clueItem.ClueHeader = header;
        clueItem.ClueBody = body;
        ClueItemsGrid.AddChild(clueItem);
    }
}

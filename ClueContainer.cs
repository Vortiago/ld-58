using System;
using System.Collections.Generic;
using Godot;

public partial class ClueContainer : PanelContainer
{
    PackedScene ClueItemScene = GD.Load<PackedScene>("res://ClueItem.tscn");

    [Export] private Button CloseButton;

    [Node("%ClueGrid")]
    private GridContainer ClueItemsGrid;

    private List<ClueItem> _clueItems = new List<ClueItem>();

    public override void _Ready()
    {
        CloseButton.Pressed += OnCloseButtonPressed;
        Visible = false;
    }

    public override void _ExitTree()
    {
        CloseButton.Pressed -= OnCloseButtonPressed;
    }

    public void CreateClueItem(string header, string body, Texture2D portrait = null)
    {
        ClueItem clueItem = (ClueItem)ClueItemScene.Instantiate();
        clueItem.ClueHeader = header;
        clueItem.ClueBody = body;
        clueItem.PortraitTexture = portrait;
        clueItem.ClueOpened += OnClueItemOpened;
        ClueItemsGrid.AddChild(clueItem);
        _clueItems.Add(clueItem);
    }

    private void OnClueItemOpened(ClueItem openedClue)
    {
        // Close all other clues when one is opened
        foreach (ClueItem clue in _clueItems)
        {
            if (clue != openedClue)
            {
                clue.Close();
            }
        }
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

    public void ClearAllClues()
    {
        // Unsubscribe from all clue events
        foreach (ClueItem clue in _clueItems)
        {
            clue.ClueOpened -= OnClueItemOpened;
            clue.QueueFree();
        }

        // Clear the list
        _clueItems.Clear();
    }
}

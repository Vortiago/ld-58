using System;
using Godot;

public partial class ClueItem : TextureButton
{
    [Node("ClueItemTextPanel")]
    private Control ClueTextContainer;

    [Node("%ClueTextHeader")]
    private Label ClueTextHeader;

    [Node("%ClueTextBody")]
    private Label ClueTextBody;

    [Export] public string ClueHeader { get; set; } = string.Empty;
    [Export] public string ClueBody { get; set; } = string.Empty;

    public override void _Ready()
    {
        ClueTextContainer.Visible = false;
        this.Pressed += OnClueItemPressed;

        ClueTextHeader.Text = ClueHeader;
        ClueTextBody.Text = ClueBody;
    }

    public override void _ExitTree()
    {
        this.Pressed -= OnClueItemPressed;
    }

    private void OnClueItemPressed()
    {
        ClueTextContainer.Visible = !ClueTextContainer.Visible;
    }
}

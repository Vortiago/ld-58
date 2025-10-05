using System;
using Godot;

public partial class ClueItem : Button
{
    [Signal]
    public delegate void ClueOpenedEventHandler(ClueItem clueItem);

    [Node("ClueItemTextPanel")]
    private Control ClueTextContainer;

    [Node("ClueItemTextPanel/MarginContainer/VBoxContainer/ClueTextHeader")]
    private Label ClueTextHeader;

    [Node("ClueItemTextPanel/MarginContainer/VBoxContainer/ClueTextBody")]
    private Label ClueTextBody;

    [Node("PortraitIcon")]
    private TextureRect PortraitIcon;

    [Export] public string ClueHeader { get; set; } = string.Empty;
    [Export] public string ClueBody { get; set; } = string.Empty;
    [Export] public Texture2D PortraitTexture { get; set; }

    public override void _Ready()
    {
        ClueTextContainer.Visible = false;
        // Make the text panel top-level so it positions relative to viewport, not parent button
        ClueTextContainer.TopLevel = true;
        this.Pressed += OnClueItemPressed;

        ClueTextHeader.Text = ClueHeader;
        ClueTextBody.Text = ClueBody;

        // Set portrait icon if provided
        if (PortraitTexture != null && PortraitIcon != null)
        {
            PortraitIcon.Texture = PortraitTexture;
        }
    }

    public override void _ExitTree()
    {
        this.Pressed -= OnClueItemPressed;
    }

    private void OnClueItemPressed()
    {
        bool wasVisible = ClueTextContainer.Visible;
        ClueTextContainer.Visible = !ClueTextContainer.Visible;

        // Emit signal when opening (not closing)
        if (!wasVisible && ClueTextContainer.Visible)
        {
            EmitSignal(SignalName.ClueOpened, this);
        }
    }

    public void Close()
    {
        ClueTextContainer.Visible = false;
        ButtonPressed = false;
    }
}

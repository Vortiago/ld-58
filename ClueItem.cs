using System;
using Godot;

public partial class ClueItem : Button
{
    [Signal]
    public delegate void ClueOpenedEventHandler(ClueItem clueItem);

    private Control ClueTextContainer;

    private Label ClueTextHeader;

    private Label ClueTextBody;

    private TextureRect PortraitIcon;

    private Button CloseButton;

    [Export] public string ClueHeader { get; set; } = string.Empty;
    [Export] public string ClueBody { get; set; } = string.Empty;
    [Export] public Texture2D PortraitTexture { get; set; }

    public override void _Ready()
    {
        ClueTextContainer = GetNode<Control>("ClueItemTextPanel");
        ClueTextHeader = GetNode<Label>("ClueItemTextPanel/MarginContainer/VBoxContainer/Header/ClueTextHeader");
        ClueTextBody = GetNode<Label>("ClueItemTextPanel/MarginContainer/VBoxContainer/ClueTextBody");
        PortraitIcon = GetNode<TextureRect>("PortraitIcon");
        CloseButton = GetNode<Button>("ClueItemTextPanel/MarginContainer/VBoxContainer/Header/CloseButton");

        ClueTextContainer.Visible = false;
        // Make the text panel top-level so it positions relative to viewport, not parent button
        ClueTextContainer.TopLevel = true;
        this.Pressed += OnClueItemPressed;
        CloseButton.Pressed += OnCloseButtonPressed;

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
        CloseButton.Pressed -= OnCloseButtonPressed;
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

    private void OnCloseButtonPressed()
    {
        Close();
    }
}

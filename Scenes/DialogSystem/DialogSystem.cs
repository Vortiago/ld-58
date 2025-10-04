using Godot;
using System;

public partial class DialogSystem : CanvasLayer
{
	[Signal] public delegate void DialogFinishedEventHandler();

	// UI Nodes
	[Export] private Control DialogBox;
	[Export] private RichTextLabel DialogTextLabel;
	[Export] private TextureRect leftPortrait;  // Inspector's portrait (hardcoded in scene)
	[Export] private TextureRect rightPortrait;  // Witness portrait (set per-frame)
	[Export] private Label leftNameLabel;
	[Export] private Label rightNameLabel;
	[Export] public Button ContinueButton;

	// Dialog data
	private string[] _currentDialogLines;
	private int _currentLineIndex = 0;
	private bool _isTyping = false;
	private float _typingSpeed = 0.01f; // Seconds per character

	public bool IsDialogActive => DialogBox != null && DialogBox.Visible;

	// Which side is speaking this sequence
	private enum SpeakerSide { Left, Right }
	private SpeakerSide _activeSide;

	public override void _Ready()
	{
		DialogBox.Visible = false;
		ContinueButton.Pressed += OnContinuePressed;
	}

	/// <summary>
	/// Starts a dialog sequence.
	/// </summary>
	/// <param name="dialogLines">Lines to display</param>
	/// <param name="leftName">Name for left speaker</param>
	/// <param name="rightName">
	/// Name for right speaker.
	/// If empty or null, right portrait & label are hidden.
	/// </param>
	/// <param name="rightPortraitTexture">Texture to display for right speaker (witness)</param>
	/// <param name="isLeftSpeaking">
	/// True if left speaks; false if right.
	/// </param>
	public void StartDialog(string[] dialogLines, string leftName, string rightName, Texture2D rightPortraitTexture, bool isLeftSpeaking)
	{
		// Set names and visibility
		leftNameLabel.Text = leftName;
		leftNameLabel.Visible = !string.IsNullOrEmpty(leftName);
		rightNameLabel.Text = rightName;
		bool hasRight = !string.IsNullOrEmpty(rightName);
		rightNameLabel.Visible = hasRight;
		rightPortrait.Visible = hasRight;

		// Set right portrait texture
		if (rightPortraitTexture != null)
		{
			rightPortrait.Texture = rightPortraitTexture;
		}

		// Determine active side
		_activeSide = isLeftSpeaking ? SpeakerSide.Left : SpeakerSide.Right;

		// Portrait modulation (use Modulate for TextureRect)
		const float inactiveAlpha = 0.4f;
		if (_activeSide == SpeakerSide.Left)
		{
			if (leftPortrait != null)
				leftPortrait.Modulate = new Color(1, 1, 1, 1);
			rightPortrait.Modulate = new Color(1, 1, 1, inactiveAlpha);
		}
		else
		{
			rightPortrait.Modulate = new Color(1, 1, 1, 1);
			if (leftPortrait != null)
				leftPortrait.Modulate = new Color(1, 1, 1, inactiveAlpha);
		}

		// Initialize dialog
		_currentDialogLines = dialogLines;
		_currentLineIndex = 0;
		DialogBox.Visible = true;
		ShowNextLine();
	}

	private void ShowNextLine()
	{
		if (_currentLineIndex >= _currentDialogLines.Length)
		{
			EndDialog();
			return;
		}
		StartTypewriter(_currentDialogLines[_currentLineIndex++]);
	}

	private void StartTypewriter(string text)
	{
		_isTyping = true;
		DialogTextLabel.Text = text;
		DialogTextLabel.VisibleRatio = 0.0f;

		ContinueButton.Text = "...";
		ContinueButton.Disabled = true;

		float duration = text.Length * _typingSpeed;
		var tween = CreateTween();
		tween.Finished += () =>
		{
			_isTyping = false;
			ContinueButton.Text = "Continue";
			ContinueButton.Disabled = false;
		};
		tween.TweenProperty(DialogTextLabel, "visible_ratio", 1.0f, duration);
	}

	private void OnContinuePressed()
	{
		if (_isTyping)
		{
			DialogTextLabel.VisibleRatio = 1.0f;
			_isTyping = false;
			ContinueButton.Text = "Continue";
			ContinueButton.Disabled = false;
		}
		else
		{
			ShowNextLine();
		}
	}

	private void EndDialog()
	{
		DialogBox.Visible = false;
		EmitSignal(SignalName.DialogFinished);
	}

	public override void _Input(InputEvent @event)
	{
		if (!DialogBox.Visible) return;
		if (@event is InputEventKey keyEvent && keyEvent.Pressed &&
		   (keyEvent.Keycode == Key.Space || keyEvent.Keycode == Key.Enter))
		{
			OnContinuePressed();
		}
	}
}

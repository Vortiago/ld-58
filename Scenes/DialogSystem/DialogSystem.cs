using Godot;
using System;

public partial class DialogSystem : CanvasLayer
{
	[Signal] public delegate void DialogFinishedEventHandler();

	// Dialog line structure with explicit speaker indicator
	public struct DialogLine
	{
		public string Text { get; set; }
		public SpeakerSide Speaker { get; set; }

		public DialogLine(string text, SpeakerSide speaker)
		{
			Text = text;
			Speaker = speaker;
		}
	}

	// Which side is speaking
	public enum SpeakerSide { Left, Right }

	// UI Nodes
	[Export] private Control DialogBox;
	[Export] private RichTextLabel DialogTextLabel;
	[Export] private TextureRect leftPortrait;  // Inspector's portrait (hardcoded in scene)
	[Export] private TextureRect rightPortrait;  // Witness portrait (set per-frame)
	[Export] private Label leftNameLabel;
	[Export] private Label rightNameLabel;
	[Export] public Button ContinueButton;

	// Dialog data
	private DialogLine[] _currentDialogLines;
	private int _currentLineIndex = 0;
	private bool _isTyping = false;
	private float _typingSpeed = 0.01f; // Seconds per character

	public bool IsDialogActive => DialogBox != null && DialogBox.Visible;

	// Speaker names
	private string _leftSpeakerName;
	private string _rightSpeakerName;

	public override void _Ready()
	{
		DialogBox.Visible = false;
		ContinueButton.Pressed += OnContinuePressed;
	}

	/// <summary>
	/// Starts a dialog sequence.
	/// </summary>
	/// <param name="dialogLines">Lines to display with explicit speaker indicators</param>
	/// <param name="rightName">
	/// Name for right speaker.
	/// If empty or null, right portrait & label are hidden.
	/// </param>
	/// <param name="rightPortraitTexture">Texture to display for right speaker (witness)</param>
	public void StartDialog(DialogLine[] dialogLines, string rightName, Texture2D rightPortraitTexture)
	{
		GD.Print($"[DEBUG] DialogSystem.StartDialog called with {dialogLines.Length} lines, rightName={rightName ?? "null"}");

		// Set names and visibility (left is always Inspector Crawford)
		_leftSpeakerName = "Inspector Crawford";
		_rightSpeakerName = rightName;
		leftNameLabel.Text = _leftSpeakerName;
		leftNameLabel.Visible = true;
		rightNameLabel.Text = rightName;
		bool hasRight = !string.IsNullOrEmpty(rightName);
		rightNameLabel.Visible = hasRight;
		rightPortrait.Visible = hasRight;

		// Set right portrait texture
		if (rightPortraitTexture != null)
		{
			rightPortrait.Texture = rightPortraitTexture;
		}

		// Initialize dialog
		_currentDialogLines = dialogLines;
		_currentLineIndex = 0;
		DialogBox.Visible = true;
		ShowNextLine();
	}

	private void ShowNextLine()
	{
		GD.Print($"[DEBUG] ShowNextLine called. Index: {_currentLineIndex}/{_currentDialogLines.Length}");
		if (_currentLineIndex >= _currentDialogLines.Length)
		{
			GD.Print("[DEBUG] No more lines - calling EndDialog");
			EndDialog();
			return;
		}

		// Get current dialog line
		DialogLine currentLine = _currentDialogLines[_currentLineIndex++];

		// Update portrait modulation to show active speaker
		const float inactiveAlpha = 0.4f;
		if (currentLine.Speaker == SpeakerSide.Left)
		{
			if (leftPortrait != null)
				leftPortrait.Modulate = new Color(1, 1, 1, 1);
			if (rightPortrait != null)
				rightPortrait.Modulate = new Color(1, 1, 1, inactiveAlpha);
		}
		else
		{
			if (rightPortrait != null)
				rightPortrait.Modulate = new Color(1, 1, 1, 1);
			if (leftPortrait != null)
				leftPortrait.Modulate = new Color(1, 1, 1, inactiveAlpha);
		}

		StartTypewriter(currentLine.Text);
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
		GD.Print("[DEBUG] DialogSystem.EndDialog called - hiding DialogBox and emitting DialogFinished signal");
		DialogBox.Visible = false;
		EmitSignal(SignalName.DialogFinished);
		GD.Print("[DEBUG] DialogFinished signal emitted");
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

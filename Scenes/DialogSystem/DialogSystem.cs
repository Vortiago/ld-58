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
	/// <param name="dialogLines">Lines to display</param>
	/// <param name="rightName">
	/// Name for right speaker.
	/// If empty or null, right portrait & label are hidden.
	/// </param>
	/// <param name="rightPortraitTexture">Texture to display for right speaker (witness)</param>
	/// <param name="isLeftSpeaking">
	/// True if left speaks; false if right.
	/// </param>
	public void StartDialog(string[] dialogLines, string rightName, Texture2D rightPortraitTexture, bool isLeftSpeaking)
	{
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

		// Toggle speaker for alternating dialog
		_activeSide = (_currentLineIndex % 2 == 0) ? SpeakerSide.Left : SpeakerSide.Right;

		// Update portrait modulation to show active speaker
		const float inactiveAlpha = 0.4f;
		if (_activeSide == SpeakerSide.Left)
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

		// Get dialog line without speaker prefix (portrait highlighting shows who's speaking)
		string dialogLine = _currentDialogLines[_currentLineIndex++];

		StartTypewriter(dialogLine);
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

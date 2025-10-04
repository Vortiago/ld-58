using Godot;
using System;

public partial class DialogSystem : CanvasLayer
{
	[Signal] public delegate void DialogFinishedEventHandler();
	
	[Export] public Control DialogBox;
	[Export] public Label CharacterNameLabel;
	[Export] public RichTextLabel DialogTextLabel;
	[Export] public Button ContinueButton;
	
	private string[] _currentDialogLines;
	private int _currentLineIndex = 0;
	private bool _isTyping = false;
	private float _typingSpeed = 0.03f; // Characters per second
	
	public override void _Ready()
	{
		DialogBox.Visible = false;
		ContinueButton.Pressed += OnContinuePressed;
		GD.Print("Dialog System initialized");
	}
	
	public void StartDialog(string characterName, string[] dialogLines)
	{
		_currentDialogLines = dialogLines;
		_currentLineIndex = 0;
		DialogBox.Visible = true;
		CharacterNameLabel.Text = characterName;
		ShowNextLine();
	}
	
	private void ShowNextLine()
	{
		if (_currentLineIndex >= _currentDialogLines.Length)
		{
			EndDialog();
			return;
		}
		
		string currentLine = _currentDialogLines[_currentLineIndex];
		StartTypewriter(currentLine);
		_currentLineIndex++;
	}

	private void StartTypewriter(string text)
	{
		_isTyping = true;
		DialogTextLabel.Text = text; // Set full text for proper layout
		DialogTextLabel.VisibleRatio = 0.0f; // Hide all text initially

		ContinueButton.Text = "...";
		ContinueButton.Disabled = true;

		// Typewriter effect using VisibleRatio
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
			return;
		}
		
		ShowNextLine();
	}
	
	private void EndDialog()
	{
		DialogBox.Visible = false;
		EmitSignal(SignalName.DialogFinished);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!DialogBox.Visible) return;
		
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Space || keyEvent.Keycode == Key.Enter)
			{
				OnContinuePressed();
			}
		}
	}
}

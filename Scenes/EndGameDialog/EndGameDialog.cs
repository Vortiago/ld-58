using Godot;
using System;

public partial class EndGameDialog : PanelContainer
{
	[Signal] public delegate void AnswerSubmittedEventHandler(int whoAnswer, int whatAnswer, int whyAnswer);

	[Export] private Button CloseButton;
	[Export] private Button SubmitButton;

	// Who options (0-3)
	[Export] private Button WhoOption1;
	[Export] private Button WhoOption2;
	[Export] private Button WhoOption3;
	[Export] private Button WhoOption4;

	// What options (0-3)
	[Export] private Button WhatOption1;
	[Export] private Button WhatOption2;
	[Export] private Button WhatOption3;
	[Export] private Button WhatOption4;

	// Why options (0-3)
	[Export] private Button WhyOption1;
	[Export] private Button WhyOption2;
	[Export] private Button WhyOption3;
	[Export] private Button WhyOption4;

	private Button[] _whoButtons;
	private Button[] _whatButtons;
	private Button[] _whyButtons;

	public override void _Ready()
	{
		// Store button arrays for easier access
		_whoButtons = new[] { WhoOption1, WhoOption2, WhoOption3, WhoOption4 };
		_whatButtons = new[] { WhatOption1, WhatOption2, WhatOption3, WhatOption4 };
		_whyButtons = new[] { WhyOption1, WhyOption2, WhyOption3, WhyOption4 };

		// Subscribe to button events
		CloseButton.Pressed += OnCloseButtonPressed;
		SubmitButton.Pressed += OnSubmitButtonPressed;

		// Initialize visibility
		Visible = false;
	}

	public override void _ExitTree()
	{
		CloseButton.Pressed -= OnCloseButtonPressed;
		SubmitButton.Pressed -= OnSubmitButtonPressed;
	}

	/// <summary>
	/// Sets whether an option is available (enabled) or locked (disabled).
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	/// <param name="available">True to enable, false to disable</param>
	public void SetOptionAvailability(int question, int option, bool available)
	{
		Button[] buttons = question switch
		{
			0 => _whoButtons,
			1 => _whatButtons,
			2 => _whyButtons,
			_ => null
		};

		if (buttons != null && option >= 0 && option < buttons.Length)
		{
			buttons[option].Disabled = !available;
		}
	}

	/// <summary>
	/// Sets the text for an option button.
	/// </summary>
	/// <param name="question">0 = Who, 1 = What, 2 = Why</param>
	/// <param name="option">Option index (0-3)</param>
	/// <param name="text">The text to display</param>
	public void SetOptionText(int question, int option, string text)
	{
		Button[] buttons = question switch
		{
			0 => _whoButtons,
			1 => _whatButtons,
			2 => _whyButtons,
			_ => null
		};

		if (buttons != null && option >= 0 && option < buttons.Length)
		{
			buttons[option].Text = text;
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

	private void OnSubmitButtonPressed()
	{
		// Get the selected option for each question (-1 if none selected)
		int whoAnswer = GetSelectedOption(_whoButtons);
		int whatAnswer = GetSelectedOption(_whatButtons);
		int whyAnswer = GetSelectedOption(_whyButtons);

		// Only submit if all questions are answered
		if (whoAnswer >= 0 && whatAnswer >= 0 && whyAnswer >= 0)
		{
			EmitSignal(SignalName.AnswerSubmitted, whoAnswer, whatAnswer, whyAnswer);
			Hide();
		}
		else
		{
			GD.Print("Please answer all questions before submitting!");
		}
	}

	private int GetSelectedOption(Button[] buttons)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].ButtonPressed)
			{
				return i;
			}
		}
		return -1;
	}
}

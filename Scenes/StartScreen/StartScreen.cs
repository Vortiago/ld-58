using Godot;
using System;

public partial class StartScreen : CanvasLayer
{
	[Signal] public delegate void GameStartedEventHandler();

	[Export] private Button StartButton;

	public override void _Ready()
	{
		StartButton.Pressed += OnStartButtonPressed;
	}

	public override void _ExitTree()
	{
		StartButton.Pressed -= OnStartButtonPressed;
	}

	private void OnStartButtonPressed()
	{
		EmitSignal(SignalName.GameStarted);
		Hide();
	}

	public new void Show()
	{
		Visible = true;
	}

	public new void Hide()
	{
		Visible = false;
	}
}

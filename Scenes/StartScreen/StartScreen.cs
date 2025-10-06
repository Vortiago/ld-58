using Godot;
using System;

public partial class StartScreen : CanvasLayer
{
	[Signal] public delegate void GameStartedEventHandler();

	[Export] private Button StartButton;
	[Export] private Button AboutButton;
	[Export] private PanelContainer AboutDialog;

	public override void _Ready()
	{
		StartButton.Pressed += OnStartButtonPressed;
		AboutButton.Pressed += OnAboutButtonPressed;
	}

	public override void _ExitTree()
	{
		StartButton.Pressed -= OnStartButtonPressed;
		AboutButton.Pressed -= OnAboutButtonPressed;
	}

	private void OnStartButtonPressed()
	{
		EmitSignal(SignalName.GameStarted);
		Hide();
	}

	private void OnAboutButtonPressed()
	{
		AboutDialog?.Show();
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

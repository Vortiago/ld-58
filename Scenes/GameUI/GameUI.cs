using Godot;
using System;

public partial class GameUI : CanvasLayer
{
	[Export] private Button CluesButton;
	[Export] private Button SolveButton;

	private ClueContainer _clueContainer;
	private EndGameDialog _endGameDialog;
	private DialogSystem _dialogSystem;

	public override void _Ready()
	{
		_dialogSystem = GetNode<DialogSystem>("/root/Main/DialogSystem");
		CluesButton.Pressed += OnCluesButtonPressed;
		SolveButton.Pressed += OnSolveButtonPressed;
	}

	public override void _ExitTree()
	{
		CluesButton.Pressed -= OnCluesButtonPressed;
		SolveButton.Pressed -= OnSolveButtonPressed;
	}

	public override void _Process(double delta)
	{
		bool dialogActive = _dialogSystem?.IsDialogActive == true;
		CluesButton.Disabled = dialogActive;
		SolveButton.Disabled = dialogActive;
	}

	public void Initialize(ClueContainer clueContainer, EndGameDialog endGameDialog)
	{
		_clueContainer = clueContainer;
		_endGameDialog = endGameDialog;
	}

	private void OnCluesButtonPressed()
	{
		if (_clueContainer != null)
		{
			_clueContainer.Show();
		}
	}

	private void OnSolveButtonPressed()
	{
		if (_endGameDialog != null)
		{
			_endGameDialog.Show();
		}
	}
}

using Godot;
using System;

public partial class Main : Node {
	[Export] PackedScene gameScene;
	[Export] CanvasLayer pauseMenu;

	private Game gameScript;
	private Label highscoreLabel;
	private Label totalCoinsLabel;

	private int highscore = 0;
	private int totaltCoins = 0;

	public override void _Ready() {
		gameScript = GetNode<Game>("Game");
		highscoreLabel = GetNode<Label>("CanvasLayer/HighscoreText");
		totalCoinsLabel = GetNode<Label>("CanvasLayer/TotalCoinsText");
	}


	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("Pause")) {
			TogglePauseGame();
		}
	}

	public void OnGameOver() {

		GD.Print("Game Over!");
		if (gameScript.currentScore > highscore) {
			highscore = gameScript.currentScore;
		}

		highscoreLabel.Text = "Highscore: " + highscore.ToString();

		totaltCoins = gameScript.collectedCoins;
		totalCoinsLabel.Text = "Total Coins: " + totaltCoins.ToString();

	    
		gameScript.QueueFree();

		var newGame = gameScene.Instantiate<Game>();
		
		newGame.OnGameOver += OnGameOver;
		AddChild(newGame);

		gameScript = newGame;
	}

	private void TogglePauseGame() {	
			GetTree().Paused = !GetTree().Paused;
			pauseMenu.Visible = !pauseMenu.Visible;
	}

	public void OnResumeButtonPressed() {
		TogglePauseGame();
	}

	public void OnQuitButtonPressed() {
		GetTree().Quit();
	}

}

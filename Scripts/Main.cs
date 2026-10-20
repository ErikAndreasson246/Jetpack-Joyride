using Godot;
using System;

public partial class Main : Node {
	[Export] PackedScene gameScene;

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
		PauseGame();
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

	private void PauseGame() {
		if (Input.IsActionJustPressed("Pause")) {
			GetTree().Paused = !GetTree().Paused;
		}
	}
}

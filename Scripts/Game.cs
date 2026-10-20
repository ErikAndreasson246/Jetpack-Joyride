using Godot;
using System;

public partial class Game : Node2D {

	[Signal] public delegate void OnGameOverEventHandler();

	[Export] CharacterBody2D character;
	[Export] PackedScene laserObstaclePackedScene;
	[Export] PackedScene missilePackedScene;
	[Export] PackedScene[] coinsPackedScenes;
	[Export] Marker2D objectSpawner;
	[Export] StaticBody2D missileIndicator;
	[Export] public float gameSpeed = 200f;
	public int collectedCoins = 0;
	public int currentScore = 0;
	private int highScore;

	private PackedScene[] spawnableObjects;
	private Label scoreCounter;
	private Label coinCounter;

	private bool playerDead = false;


	public override void _Ready() {
		playerDead = false;

		scoreCounter = GetNode<Label>("CanvasLayer/ScoreText");
		coinCounter = GetNode<Label>("CanvasLayer/CoinsText");

		var objectSpawnTimer = GetNode<Timer>("ObjectSpawnTimer");
		objectSpawnTimer.Timeout += SpawnObject;
		objectSpawnTimer.Start();
		
		var scoreTimer = GetNode<Timer>("ScoreTimer");
		scoreTimer.Timeout += UpdateScore;
		scoreTimer.Start();

		var respawnTimer = GetNode<Timer>("RespawnTimer");
		respawnTimer.Timeout += RespawnPlayer;
		respawnTimer.Stop();
	}

	public override void _Process(double delta) {
		
	}

	public override void _PhysicsProcess(double delta) {
		if (playerDead && gameSpeed > 0) {
			gameSpeed -= 1f;
		}
		missileIndicator.Position = new Vector2(1150, character.Position.Y);
	}

	public void OnPlayerDeath() {
		playerDead = true;

		var timer = GetNode<Timer>("RespawnTimer");
		timer.Start();
	}

	

	private void SpawnObject() {
		
		SpawnMissile();

		PackedScene scene;

		if (GD.Randf() < 0.5f) {
			scene = laserObstaclePackedScene;
		}
		else {
        	scene = coinsPackedScenes[
            GD.RandRange(0, coinsPackedScenes.Length - 1)];
    	}

		var spawnedObject = scene.Instantiate<Node2D>();

		Vector2 objectPos = objectSpawner.Position;

		objectPos.X = objectSpawner.Position.X;
		objectPos.Y = objectSpawner.Position.Y + GD.RandRange(-200, 200);
		if (spawnedObject is LaserObstacle) {
			spawnedObject.Rotation = (float)GD.RandRange(-2f, 2f);
		}
		spawnedObject.Position = objectPos;
		AddChild(spawnedObject);
	}

	private void RespawnPlayer() {
		playerDead = false;
		EmitSignal(SignalName.OnGameOver);
	}

	private void UpdateScore() {
		currentScore++;
		highScore = currentScore;
		scoreCounter.Text = "Score: " + currentScore.ToString();
	}

	public void OnCoinCollected() {
		collectedCoins++;
		coinCounter.Text = "Coins: " + collectedCoins.ToString();
	}

	private void SpawnMissile() {
		
		int spawnChance =GD.RandRange(1, 3);
		if (spawnChance == 1) {
			missileIndicator.Visible = true;

			var timer = GetNode<Timer>("MissileSpawnTimer");
			timer.Timeout += () => {
				missileIndicator.Visible = false;
				var missile = missilePackedScene.Instantiate<Missile>();
				Vector2 missilePos = missileIndicator.Position;
				missilePos.X = 1200;
				missilePos.Y = missileIndicator.Position.Y;
				missile.Position = missilePos;
				AddChild(missile);
				timer.Stop();
			};
			timer.Start();
		}
	}
}

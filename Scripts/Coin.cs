using Godot;
using System;

public partial class Coin : StaticBody2D {

	private Game gameScript;

	public override void _Ready() {
		gameScript = GetParent().GetParent<Game>();
	}

	public override void _Process(double delta) {

		if (Position.X < -3000)  QueueFree(); 
	}

	public override void _PhysicsProcess(double delta) {

		Vector2 pos = Position;
		pos.X -= gameScript.gameSpeed * (float)delta;
		Position = pos;
	}
}

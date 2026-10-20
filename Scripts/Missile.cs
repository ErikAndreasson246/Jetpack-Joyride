using Godot;
using System;

public partial class Missile : StaticBody2D {
	private Game gameScript;

	public override void _Ready() {
		gameScript = GetParent<Game>();
	}

	public override void _Process(double delta) {
		if (Position.X < -200) QueueFree();
	}

	public override void _PhysicsProcess(double delta) {

		Vector2 pos = Position;
		pos.X -= gameScript.gameSpeed * 1.5f * (float)delta;
		Position = pos;
	}
}
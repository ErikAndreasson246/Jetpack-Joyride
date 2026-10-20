using Godot;
using System;

public partial class LaserObstacle : StaticBody2D {

	private Game gameScript;
	bool isRotating;

	public override void _Ready() {
		gameScript = GetParent<Game>();
		isRotating = GD.RandRange(0, 2) == 1;
	}

	public override void _Process(double delta) {
		if (Position.X < -200) QueueFree();
		
		if (isRotating) {
			Rotation += 1.5f * (float)delta;
		}
	}

	public override void _PhysicsProcess(double delta) {

		Vector2 pos = Position;
		pos.X -= gameScript.gameSpeed * (float)delta;
		Position = pos;
	}
}
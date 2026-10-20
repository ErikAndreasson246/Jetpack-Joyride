using Godot;
using System;

public partial class CharacterScript : CharacterBody2D {

	[Signal] public delegate void OnPlayerDeathEventHandler();
	[Signal] public delegate void OnCoinCollectedEventHandler();


	[Export] private float jetpackStrenght = -2000f;

	private float maxVelocityY = -500f;
	private float minVelocityY = 500f;

	private bool canMove = true;

	private Vector2 mVelocity;
	
	public override void _Ready() {
		canMove = true;
	}

	public override void _Process(double delta) {
		
	}

	public override void _PhysicsProcess(double delta) {

		mVelocity = Velocity;
		mVelocity.Y += GetGravity().Y * (float)delta;

		if (Input.IsActionPressed("Fly") && canMove) {
			mVelocity.Y += jetpackStrenght * (float)delta;
		}

		if (mVelocity.Y < maxVelocityY) {
			mVelocity.Y = maxVelocityY;
		} else if (mVelocity.Y > minVelocityY) {
			mVelocity.Y = minVelocityY;
		}
		
		Velocity = mVelocity;
		MoveAndSlide();
	}


	private void PlayerDeath(){
		canMove = false;
		EmitSignal(SignalName.OnPlayerDeath);
	}

	public void OnPlayerHit(Rid body_rid, Node2D body, int body_shape_index, int local_shape_index) {
		GD.Print("Player hit by: " + body_shape_index);
		if (body is LaserObstacle || body is Missile) {
			PlayerDeath();
			body.GetChild<CollisionShape2D>(body_shape_index).Disabled = true;
		}
	}

	public void OnCoinHit(Area2D area) {
		if (area.GetParent() is Coin) {
			EmitSignal(SignalName.OnCoinCollected);
			area.GetParent().QueueFree();
		}
	}
}

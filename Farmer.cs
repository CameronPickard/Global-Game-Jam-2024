using Godot;
using System;

public partial class Farmer : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

	public Vector2 ScreenSize; // Size of the game window.

	public GpuParticles2D WaterParticles;
	public void _Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;

		//WaterParticles = GetNode<GpuParticles2D>("WaterParticles");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		//Hide(); uncomment this later....
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		//var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			//animatedSprite2D.Play();
		}
		else
		{
			//animatedSprite2D.Stop();
		}

		//Dont use this.
		/*Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);*/

		Velocity = velocity;
		MoveAndSlide();
		if (velocity.X != 0)
		{
			//animatedSprite2D.Animation = "walk";
			//animatedSprite2D.FlipV = false;
			// See the note below about boolean assignment.
			//animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			//animatedSprite2D.Animation = "up";
			//animatedSprite2D.FlipV = velocity.Y > 0;
		}

		bool shootingWater = Input.IsActionPressed("shoot_water");
		if(shootingWater) 
		{
			//Ver 1 - using particles
			//WaterParticles.Emitting = shootingWater;

			//Ver 2 - Instantiate each drop
			PackedScene waterDropScene = GD.Load<PackedScene>("res://Scenes/WaterDrop.tscn");

			WaterDrop waterDrop = waterDropScene.Instantiate<WaterDrop>();

			waterDrop.ShootWaterDrop(Position, WaterDrop.Directions.Down);

			Window root = GetTree().Root;
			root.AddChild(waterDrop);
		}

	}
}

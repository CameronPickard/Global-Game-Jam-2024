using Godot;
using System;

public partial class Farmer : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 250; // How fast the player will move (pixels/sec).

	public Vector2 ScreenSize; // Size of the game window.

	public HelperMethods.Directions currentDirection = HelperMethods.Directions.Down;

	public AnimationPlayer farmerAnimationPlayer;
	public Sprite2D farmerSprite;
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
		farmerAnimationPlayer = GetNode<AnimationPlayer>("Animations");
		farmerSprite = GetNode<Sprite2D>("AnimatedSprite2D");
		//Hide(); uncomment this later....
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.
		var isMoving = false;
		bool isGameOver = GameManager.Instance.gameState == GameManager.GameState.GameOver || GameManager.Instance.gameState == GameManager.GameState.Victory;
		if(isGameOver) 
		{
			string endAnimation = GameManager.Instance.gameState == GameManager.GameState.Victory ? "walking_down" : "idle";
			//dont allow the farmer to do anything
			farmerAnimationPlayer.Play(endAnimation);
			if (farmerSprite != null)
			{
				farmerSprite.FlipH = false;
			}
			return;
		}
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
			currentDirection = HelperMethods.Directions.Right;
			isMoving = true;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
			currentDirection = HelperMethods.Directions.Left;
			isMoving = true;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
			currentDirection = HelperMethods.Directions.Down;
			isMoving = true;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
			currentDirection = HelperMethods.Directions.Up;
			isMoving = true;
		}

		//var animatedSprite2D = GetNode<Sprite2D>("AnimatedSprite2D");

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
		if (shootingWater)
		{
			//Ver 1 - using particles
			//WaterParticles.Emitting = shootingWater;

			//Ver 2 - Instantiate each drop
			PackedScene waterDropScene = GD.Load<PackedScene>("res://Scenes/WaterDrop.tscn");

			WaterDrop waterDrop = waterDropScene.Instantiate<WaterDrop>();

			waterDrop.ShootWaterDrop(Position, currentDirection);

			Window root = GetTree().Root;
			root.AddChild(waterDrop);
		}

		//Animation
		string animationName = "idle";
		bool shouldFlipH = false;
		if (shootingWater)
		{
			if (isMoving)
			{
				switch (currentDirection)
				{
					case HelperMethods.Directions.Up:
						animationName = "walking_up_atk";
						break;
					case HelperMethods.Directions.Down:
						animationName = "walking_down_atk";
						break;
					case HelperMethods.Directions.Right:
						animationName = "walking_side_atk";
						break;
					case HelperMethods.Directions.Left:
						animationName = "walking_side_atk";
						shouldFlipH = true;
						break;
				}
			}
			else
			{
				switch (currentDirection)
				{
					case HelperMethods.Directions.Up:
						animationName = "standing_atk_up";
						break;
					case HelperMethods.Directions.Down:
						animationName = "standing_atk_down";
						break;
					case HelperMethods.Directions.Right:
						animationName = "standing_atk_side";
						break;
					case HelperMethods.Directions.Left:
						animationName = "standing_atk_side";
						shouldFlipH = true;
						break;
				}
			}
		}
		else
		{
			if (isMoving)
			{
				switch (currentDirection)
				{
					case HelperMethods.Directions.Up:
						animationName = "walking_up";
						break;
					case HelperMethods.Directions.Down:
						animationName = "walking_down";
						break;
					case HelperMethods.Directions.Right:
						animationName = "walking_side";
						break;
					case HelperMethods.Directions.Left:
						animationName = "walking_side";
						shouldFlipH = true;
						break;
				}
			}
			else
			{
				switch (currentDirection)
				{
					case HelperMethods.Directions.Up:
						animationName = "standing_atk_up";
						break;
					case HelperMethods.Directions.Down:
						animationName = "standing_atk_down";
						break;
					case HelperMethods.Directions.Right:
						animationName = "standing_atk_side";
						break;
					case HelperMethods.Directions.Left:
						animationName = "standing_atk_side";
						shouldFlipH = true;
						break;
				}
			}
		}
		if(farmerAnimationPlayer != null) 
		{
			farmerAnimationPlayer.Play(animationName);
		}
		if(farmerSprite != null) {
			farmerSprite.FlipH = shouldFlipH;
		}
	}

	public enum FarmerState {
		StandingAtkDown,
		StandingAtkSide,
		StandingAtkUp,
		WalkingDown,
		WalkingDownAtk,
		WalkingSide,
		WalkingSideAtk,
		WalkingUp,
		WalkingUpAtk
	}
}

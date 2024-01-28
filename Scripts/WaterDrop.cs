using Godot;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;

public partial class WaterDrop : Area2D
{
	public bool hasBeenShot = false;

	public double angleOfDirection = 0f;
	public double mathAngle
	{

		get { return angleOfDirection * (Math.PI / 180); }
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(hasBeenShot) {
			var velocity = Vector2.Zero; // The player's movement vector.
			velocity.Y += 2;
			Position = new Vector2(Position.X + (float)Math.Cos(mathAngle), Position.Y + (float)Math.Sin(mathAngle));
		}
	}

	public void ShootWaterDrop(Vector2 startPosition, HelperMethods.Directions direction) 
	{
		//hasBeenShot = true;
		hasBeenShot = true;
		Position = new Vector2(startPosition.X, startPosition.Y + 5);
		
		switch(direction) {
			case HelperMethods.Directions.Up:
				angleOfDirection = 270f;
				break;
			case HelperMethods.Directions.Down:
				angleOfDirection = 90f;
				break;
			case HelperMethods.Directions.Right:
				angleOfDirection = 0f;
				break;
			case HelperMethods.Directions.Left:
				angleOfDirection = 180f;
				break;
		}
		angleOfDirection = GetRandomNumber(angleOfDirection - 30, angleOfDirection + 30);
		Timer timer = GetNode<Timer>("WaterDropTimer");
		timer.WaitTime = GetRandomNumber(timer.WaitTime - .05, timer.WaitTime + .05);
		Position = new Vector2(startPosition.X + (float)Math.Cos(mathAngle), startPosition.Y + (float)Math.Sin(mathAngle));
		Debug.Print("Water drop created");
	}

	private double GetRandomNumber(double minimum, double maximum)
	{
		Random random = new Random();
		return random.NextDouble() * (maximum - minimum) + minimum;
	}

	private void OnWaterDropTimerTimeout()
	{
		Debug.Print("Timer out yay");
		QueueFree();
	}

	private void OnWaterDropAreaEntered(Area2D area)
	{
		if (area.IsInGroup("PlantWaterIntake"))
		{
			QueueFree();
		}
	}
}

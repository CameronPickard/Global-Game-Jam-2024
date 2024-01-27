using Godot;
using System;
using System.Diagnostics;

public partial class WaterDrop : Area2D
{
	public bool hasBeenShot = false;

	public double angleOfDirection = 180f;
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

	public void ShootWaterDrop(Vector2 startPosition, Directions direction) 
	{
		//hasBeenShot = true;
		hasBeenShot = true;
		Position = new Vector2(startPosition.X, startPosition.Y + 5);
		//Create the water drop timer
		//Timer timer = GetNode<Timer>("WaterDropTimer");
		//timer.Connect("timeout", new Callable(this, nameof(this.OnWaterDropTimeout)));
		//timer.Start();
		angleOfDirection = GetRandomNumber(180 - 20, 180 + 20);
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

	public enum Directions {
		Left,
		Right,
		Up,
		Down

	}

}

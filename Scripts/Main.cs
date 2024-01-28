using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene BunnyScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("BunnySpawnTimer").Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBunnySpawnTimerTimeout()
	{
		// Create a new instance of the Bunny scene.
		Bunny bunny = BunnyScene.Instantiate<Bunny>();

		// Choose a random location on Path2D.
		var bunnySpawnLocation = GetNode<PathFollow2D>("BunnySpawnPath/BunnySpawnLocation");
		bunnySpawnLocation.ProgressRatio = GD.Randf();

		// Set the bunny's position to a random location.
		bunny.Position = bunnySpawnLocation.Position;

		// Spawn the bunny by adding it to the Main scene.
		AddChild(bunny);
	}
}

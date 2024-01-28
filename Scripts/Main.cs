using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene BunnyScene { get; set; }
	public bool hasGameOverHappened = false;
	public ColorRect gameEndRect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("BunnySpawnTimer").Start();
		gameEndRect = GetNode<ColorRect>("GameEndRect");
		gameEndRect.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GameManager.Instance.gameState == GameManager.GameState.Victory) 
		{
			if(!hasGameOverHappened) {
				var allBunnies = GetTree().GetNodesInGroup("Bunnies");
				foreach(var bunny in allBunnies) {
					bunny.QueueFree();
				}
				hasGameOverHappened = true;
			}
			gameEndRect.Show();
		}
		else if(GameManager.Instance.gameState == GameManager.GameState.GameOver) 
		{
			gameEndRect.Show();
		}
	}

	private void OnBunnySpawnTimerTimeout()
	{
		if(GameManager.Instance.gameState != GameManager.GameState.Victory) 
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
}

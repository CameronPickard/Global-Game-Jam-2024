using Godot;
using System;

public partial class GameManager : Node
{dx3
	public ProgressBar gardenGrowthBar { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gardenGrowthBar = GetNode<ProgressBar>("UI/GardenGrowthBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(gardenGrowthBar!=null) {
			gardenGrowthBar.Value += 0.01;
		}
	}


}

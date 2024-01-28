using Godot;
using System;
using System.Diagnostics;

public partial class UI : Control
{
	public ProgressBar gardenGrowthBar { get; set; }
	public ProgressBar enemyEatingBar { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gardenGrowthBar = GetNode<ProgressBar>("GardenGrowthBar");
		enemyEatingBar = GetNode<ProgressBar>("EnemyEatingBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(gardenGrowthBar!=null) {
			gardenGrowthBar.Value = GameManager.Instance.getGardenPoints();
			//Debug.Print("Garden Growth: " + gardenGrowthBar.Value);
		}
		if (enemyEatingBar != null)
		{
			enemyEatingBar.Value = GameManager.Instance.getEnemyEatingPoints();
			//Debug.Print("Garden Growth: " + gardenGrowthBar.Value);
		}
	}
}

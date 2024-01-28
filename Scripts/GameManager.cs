using Godot;
using System;

public partial class GameManager : Node
{

	public double gardenGrowthPoints = 0;
	public double enemyEatingPoints = 0;
	private static GameManager _instance;
	public static GameManager Instance => _instance;
	// Use _EnterTree to make sure the Singleton instance is avaiable in _Ready()
	public override void _EnterTree()
	{
		if (_instance != null)
		{
			this.QueueFree(); // The Singletone is already loaded, kill this instance
		}
		_instance = this;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public double getGardenPoints() { return gardenGrowthPoints; }
	public double getEnemyEatingPoints() { return enemyEatingPoints; }

	public void OnPlantWaterDropReceived() {
		//gardenGrowthPoints += 0.1;
	}

	public void OnPlantSparkling()
	{
		gardenGrowthPoints += 0.02;
	}

	public void OnEnemyEating()
	{
		enemyEatingPoints += 0.08;
	}
}
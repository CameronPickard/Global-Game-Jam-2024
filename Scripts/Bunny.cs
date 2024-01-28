using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Bunny : Area2D
{
	private static float _bunnySpeed = 14.0f;
	private static float _bunnyRunawaySpeed = 100.0f;

	public BunnyState currentState;
	public double bunnyHealth = 200;
	public Area2D targetPlant = null;
	//Position of the plant that the bunny should be moving towards
	public Vector2 targetPosition;

	//Plant being eaten
	public Area2D plantBeingEaten = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentState = BunnyState.MovingIn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(bunnyHealth <= 0) {
			currentState = BunnyState.RunningAway;
		}
		if (currentState == BunnyState.RunningAway)
		{
			//Bunny should be moving in towards its chosen plant
			Vector2 normalToPlant = (targetPosition - GlobalPosition).Normalized();
			Vector2 normalAwayFromPlant = normalToPlant * -1;
			normalAwayFromPlant = new Vector2(normalAwayFromPlant.X * (float)delta * _bunnyRunawaySpeed, normalAwayFromPlant.Y * (float)delta * _bunnyRunawaySpeed);
			Position = new Vector2(Position.X + normalAwayFromPlant.X, Position.Y + normalAwayFromPlant.Y);
		}
		else if(currentState == BunnyState.Eating) 
		{
			GameManager.Instance.OnEnemyEating();
			if(plantBeingEaten != null)
			{
				plantBeingEaten.Call("OnGettingEaten");
			}
		}
		else if (targetPlant == null)
		{
			//gotta pick a plant to target
			var allPlants = GetTree().GetNodesInGroup("PlantWaterIntake");
			if(allPlants!= null)
			{
				Area2D currentPlantBeingEvaluated;
				float shortestDistanceTo = float.MaxValue;
				float distanceBeingEvaluated;
				foreach(Node plantNode in allPlants) {
					currentPlantBeingEvaluated = plantNode.GetNode<Area2D>("."); //Get the Area2D so we can look at its position

					distanceBeingEvaluated = GlobalPosition.DistanceTo(currentPlantBeingEvaluated.GlobalPosition);
					if(distanceBeingEvaluated < shortestDistanceTo) 
					{
						shortestDistanceTo = distanceBeingEvaluated;
						targetPlant = currentPlantBeingEvaluated;
						targetPosition = targetPlant.GlobalPosition;
					}
				}
			}
		}
		else if (currentState == BunnyState.MovingIn) {
			//Bunny should be moving in towards its chosen plant
			Vector2 normalToPlant = (targetPosition - GlobalPosition).Normalized();
			normalToPlant = new Vector2(normalToPlant.X * (float)delta * _bunnySpeed, normalToPlant.Y * (float)delta * _bunnySpeed);
			Position = new Vector2(Position.X + normalToPlant.X, Position.Y + normalToPlant.Y);
		}
	}

	private void OnBunnyAreaEntered(Area2D area)
	{
		if (area.IsInGroup("PlantWaterIntake"))
		{
			currentState = BunnyState.Eating;
			plantBeingEaten = area;
		}
		if (area.IsInGroup("WaterDrop")) {
			bunnyHealth -= 5;
		}
	}

	public enum BunnyState
	{
		MovingIn,
		Eating,
		RunningAway
	}
}

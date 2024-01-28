using Godot;
using System;
using System.Diagnostics;

public partial class PlantWaterIntake : Area2D
{
	public double currentWaterCount = 0;
	public double sparkleWaterCount = 100;
	public double drowningWaterCount = 10000; //scrapped feature - just set it super high so it's not a problem

	public Timer sparkleTimer = null;
	public GpuParticles2D sparkleParticles = null;
	public AnimationPlayer plantAnimationPlayer;
	public bool IsSparkling
	{
		get { return (currentWaterCount > sparkleWaterCount && currentWaterCount < drowningWaterCount); }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sparkleTimer = GetNode<Timer>("../SparkleTimer");
		sparkleParticles = GetNode<GpuParticles2D>("../SparkleParticles");
		plantAnimationPlayer = GetNode<AnimationPlayer>("../Animations");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(IsSparkling) {
			GameManager.Instance.OnPlantSparkling();
			sparkleParticles.Emitting = true;
			plantAnimationPlayer.Play("idle_watered");
		}
		else {
			plantAnimationPlayer.Play("idle_unwatered");
		}
	}

	private void OnPlantWaterIntakeAreaEntered(Area2D area)
	{
		if(area.IsInGroup("WaterDrop")) {
			Debug.Print("Watered amount: " + this.currentWaterCount);

			bool wasSparkling = false;
			currentWaterCount += 1.5;
			if(!wasSparkling && IsSparkling) {
				sparkleTimer.Start();
			}
		}
		
	}

	private void OnGettingEaten() 
	{
		OnSparkleEnd();
	}

	private void OnSparkleEnd() {
		currentWaterCount = 0;
		sparkleParticles.Emitting = false;
	}
}

using Godot;
using System;
using System.Diagnostics;

public partial class UI : Control
{
	public TextureProgressBar gardenGrowthBar { get; set; }
	public TextureProgressBar enemyEatingBar { get; set; }

	public Label VictoryLabel { get; set; }
	public Label GameOverLabel { get; set; }
	public Label StartLabel { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gardenGrowthBar = GetNode<TextureProgressBar>("HBoxContainer/GardenGrowthBar");
		enemyEatingBar = GetNode<TextureProgressBar>("HBoxContainer/EnemyEatingBar");

		VictoryLabel = GetNode<Label>("VictoryLabel");
		GameOverLabel = GetNode<Label>("GameOverLabel");
		StartLabel = GetNode<Label>("StartLabel");

		GameOverLabel.Hide();
		VictoryLabel.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(GameManager.Instance.gameState == GameManager.GameState.InProgress) 
		{
			if (gardenGrowthBar != null)
			{
				gardenGrowthBar.Value = GameManager.Instance.getGardenPoints();
				//Debug.Print("Garden Growth: " + gardenGrowthBar.Value);
			}
			if (enemyEatingBar != null)
			{
				enemyEatingBar.Value = GameManager.Instance.getEnemyEatingPoints();
				//Debug.Print("Garden Growth: " + gardenGrowthBar.Value);
			}
		}
		else
		{
			if (GameManager.Instance.gameState == GameManager.GameState.Victory) 
			{
				StartLabel.Hide();
				GameOverLabel.Hide();
				VictoryLabel.Show();
			}
			else if(GameManager.Instance.gameState == GameManager.GameState.GameOver) 
			{
				StartLabel.Hide();
				GameOverLabel.Show();
				VictoryLabel.Hide();
			}
		}
	}
}

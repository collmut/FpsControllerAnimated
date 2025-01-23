using Godot;
using System;

public partial class EarthquakeArea : Area3D
{
	[Export]
	bool ContinousShake = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void BodyEnteredArea(Node body)
	{
		if(body is Player)
		{
			Node cam = body.GetNode("cam");
			
			if(ContinousShake == false)
			{
				cam.Call(method: "ShakeOnce");
			}
			else
			{
				cam.Call(method: "ShakeContinous");
			}
		}
	}
	
	public void BodyExitedArea(Node body)
	{
		if(body is Player)
		{
			Node cam = body.GetNode("cam");
			cam.Call(method: "OnEarthquakeAreaExited");
		}
	}
}

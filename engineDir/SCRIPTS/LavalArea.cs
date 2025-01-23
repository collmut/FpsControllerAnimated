using Godot;
using System;

public partial class LavalArea : Area3D
{
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
			body.Call(method: "Die");
		}
	}
}

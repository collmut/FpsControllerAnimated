using Godot;
using System;

public partial class PlayerInputManager : Node3D
{
	Node player;
	float MouseSensitivity = 0.4f;
	bool StoppedMoving = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode("../../");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckInputActions();
	}
	
	void CheckInputActions()
	{
		CheckMovementInputs();
		
		if(Input.IsActionPressed("jump"))
		{
			player.Call(method: "Jump");
		}
	}
	
	void CheckMovementInputs()
	{
		Vector2 moveDir = new Vector2();
		
		if(Input.IsActionPressed("forward"))
		{
			moveDir.Y = 1;
		}
		
		if(Input.IsActionPressed("back"))
		{
			moveDir.Y = -1;
		}
		
		if(Input.IsActionPressed("left"))
		{
			moveDir.X = 1;
		}
		
		if(Input.IsActionPressed("right"))
		{
			moveDir.X = -1;
		}
		
		player.Call(method: "Move", moveDir);
		
		// check stop : 
		if(Input.IsActionPressed("forward") == false && Input.IsActionPressed("back") == false && Input.IsActionPressed("left") == false && Input.IsActionPressed("right") == false)
		{
			if(StoppedMoving == false)
			{
				player.Call(method: "OnMoveStop");
				StoppedMoving = true;
			}
		}
		else
		{
			// movement input is detected : 
			StoppedMoving = false;
		}
		
	}
	
	public override void _Input(InputEvent ev)
	{
		if(ev is InputEventMouseMotion)
		{
			InputEventMouseMotion mm = (InputEventMouseMotion)ev;
			player.Call(method: "Look", mm.Relative, MouseSensitivity);
			
		}
	}
}

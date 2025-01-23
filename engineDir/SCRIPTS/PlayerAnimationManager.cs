using Godot;
using System;

public partial class PlayerAnimationManager : Node3D
{
	[Export]
	AnimationPlayer anim;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Callable cl = new Callable(target: this, method: "OnAnimFinished");
		anim.Connect(signal: "animation_finished", callable: cl);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public void Run()
	{
		if(PlayerHasDied() == true) return;
		
		// run animation : 
		anim.Play(name: "run" , customBlend: 0.5f);
	}
	
	public void Idle()
	{
		if(PlayerHasDied() == true) return;
		
		// idle animation : 
		anim.Play(name: "idle" , customBlend: 0.5f);
	}
	
	public void Die()
	{
		// die animation : 
		anim.Play(name: "die" , customBlend: 0.5f);
	}
	
	public void Jump()
	{
		if(PlayerHasDied() == true) return;
		
		// jump animation : 
		anim.Play(name: "jump" , customBlend: 0.5f);
	}
	
	public void Land()
	{
		if(PlayerHasDied() == true) return;
		
		// land animation : 
		anim.Play(name: "land" , customBlend: 0.5f);
	}
	
	public void OnAnimFinished(string animationName)
	{
		switch(animationName)
		{
			case "land" : 
				Idle();
			break;
			default:
			break;
		}
	}
	
	bool PlayerHasDied()
	{
		Node player = GetNode("../../");
		bool hasDied = (bool)player.Get(property: "HasDied");
		
		return hasDied;
	}
}

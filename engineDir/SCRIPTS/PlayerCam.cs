using Godot;
using System;

public partial class PlayerCam : Node3D
{
	[Export]
	Node3D swayCamParent;
	
	Vector3 InitialSwayRot;
	Vector3 TargetSwayRot;
	float MaxSway = 5f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitialSwayRot = swayCamParent.RotationDegrees;
		TargetSwayRot = InitialSwayRot;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		InterpolateSway();
	}
	
	public void MoveSway(Vector2 moveDirection)
	{
		// move direction in the x and y axis is either 0 or -1 or 1
		
		TargetSwayRot.X = InitialSwayRot.X + (moveDirection.Y * MaxSway);
		TargetSwayRot.Z = InitialSwayRot.Z + (moveDirection.X * MaxSway);
	}
	
	void InterpolateSway()
	{
		Vector3 newRot = swayCamParent.RotationDegrees;
		newRot = newRot.Lerp(to: TargetSwayRot, weight: 0.1f);
		
		swayCamParent.RotationDegrees = newRot;
	}
	
	public void ShakeOnce()
	{
		AnimationPlayer anim = (AnimationPlayer)GetNode("AnimationPlayer");
		anim.Play(name: "shake_once" , customBlend: 0.5f);
	}
	
	public void ShakeContinous()
	{
		AnimationPlayer anim = (AnimationPlayer)GetNode("AnimationPlayer");
		anim.Play(name: "shake_continous" , customBlend: 0.5f);
	}
	
	public void OnEarthquakeAreaExited()
	{
		AnimationPlayer anim = (AnimationPlayer)GetNode("AnimationPlayer");
		anim.Play(name: "RESET", customBlend: 0.5f);
	}

}

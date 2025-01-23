using Godot;
using System;

public partial class Player : CharacterBody3D
{
	Node3D cam;
	Node animationManager;
	
	Vector3 TargetRot;
	Vector3 CamTargetRot;
	float CamMaxRotX = 65f;
	Vector2 MoveDirection = new();

	float SpeedZ = 10f;
	float SpeedX = 10f;
	Vector3 TargetVelocity;
	float GravityAcceleration = 0.5f;
	public bool HasDied = false;
	float JumpForce = 15f;
	int JumpCount = 0;
	int MaxJumps = 1;
	bool AlreadyOnFloor = false;
	bool StoppedMoving = false;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = (Node3D)GetNode("cam");
		animationManager = GetNode("utils/AnimationManager");
		
		TargetRot = this.RotationDegrees;
		CamTargetRot = cam.RotationDegrees;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		InterpolateCamRot();
		InterpolateRot();
		InterpolateVelocity();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		ApplyMovementVelocity();
		ApplyGravity();
		
		MoveAndSlide();
	}
	
	public void Move(Vector2 moveDir)
	{
		if(HasDied == true) return;
		if(IsOnFloor() == false) return;
		
		//
		MoveDirection = moveDir;
		
		// cam sway : 
		cam.Call(method: "MoveSway", moveDir);
		
		if(GetUnsignedFloat(moveDir.X) > 0 || GetUnsignedFloat(moveDir.Y) > 0)
		{
			// run animation when player is moving in either direction : 
			animationManager.Call(method: "Run");
		}
	}
	
	public void OnMoveStop()
	{
		if(HasDied == true) return;
		if(IsOnFloor() == false) return;
		
		// stop run : 
		animationManager.Call(method: "Idle");
	}
	
	public void Jump()
	{
		if(HasDied == true) return;
		if(JumpCount >= MaxJumps) return;
		
		Vector3 newVel = Velocity;
		newVel.Y = JumpForce;
		
		Velocity = newVel;
		MoveAndSlide();
		
		// 
		JumpCount = JumpCount + 1;
		
		// jump animation : 
		animationManager.Call(method: "Jump");
	}
	
	void ApplyMovementVelocity()
	{
		if(HasDied == true) return;
		
		Vector2 moveDirNormalized = MoveDirection.Normalized();
		
		float InitialYVel = Velocity.Y;
		Vector3 forwardVel = GlobalTransform.Basis.Z * moveDirNormalized.Y * SpeedZ * -1;
		Vector3 sideVel = GlobalTransform.Basis.X * moveDirNormalized.X * SpeedX * -1;
		
		TargetVelocity = forwardVel + sideVel;
		TargetVelocity.Y = InitialYVel;
	}
	
	void ApplyGravity()
	{
		Vector3 newVel = Velocity;
		
		if(IsOnFloor() == false)
		{
			newVel.Y = newVel.Y - GravityAcceleration;
			AlreadyOnFloor = false;
		}
		else
		{
			if(AlreadyOnFloor == false)
			{
				JustEnteredFloor();
				AlreadyOnFloor = true;
			}
			
			newVel.Y = 0;
			JumpCount = 0;
		}
		
		Velocity = newVel;
	}
	
	public void Look(Vector2 lookRel, float mouseSensitivity)
	{
		// look in the y axis : 
		TargetRot.Y = TargetRot.Y - (lookRel.X * mouseSensitivity / 2);
		
		// camera look in the x axis :  
		CamTargetRot.X = CamTargetRot.X - lookRel.Y;
		CamTargetRot.X = Mathf.Clamp(value: CamTargetRot.X, min: -CamMaxRotX, max: CamMaxRotX);
	}
	
	void InterpolateRot()
	{
		float targetRotYRad = Mathf.DegToRad(TargetRot.Y);
		
		Vector3 newRotRad = this.Rotation;
		newRotRad.Y = Mathf.LerpAngle(from: newRotRad.Y , to: targetRotYRad, weight: 0.1f);
		
		this.Rotation = newRotRad;
	}
	
	void InterpolateCamRot()
	{
		Vector3 newRot = cam.RotationDegrees;
		newRot = newRot.Lerp(to: CamTargetRot, weight: 0.05f);
		
		cam.RotationDegrees = newRot;
	}
	
	void InterpolateVelocity()
	{
		// interpolate velocity : 
		Vector3 newVel = Velocity;
		newVel = newVel.Lerp(to: TargetVelocity , weight: 0.1f);
		Velocity = newVel;
	}
	
	float GetUnsignedFloat(float f)
	{
		if(f < 0) return f * -1;
		return f;
	}
	
	void JustEnteredFloor()
	{
		// land animation : 
		animationManager.Call(method: "Land");
	}
	
	public void Die()
	{
		if(HasDied == true) return;
		
		// die animation : 
		animationManager.Call(method: "Die");
		
		// stop player : 
		TargetVelocity = new Vector3();
		
		// set died : 
		HasDied = true;
	}
}

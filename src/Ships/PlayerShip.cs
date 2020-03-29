using Godot;
using System;

public class PlayerShip : KinematicBody2D
{
	private RayCast2D headingCast2D;
	private ShipPilot pilot;
	private Node2D frontExhaust;
	private Node2D backExhaust;

	[Export]
	public float MaxThrust {
		get {
			return pilot.MaxThrust;
		}
		set {
			pilot.MaxThrust = Mathf.Abs(value);
		}
	}

	[Export]
	public float MaxSpeed { 
		get {
			return pilot.MaxSpeed;
		}
		set {
			pilot.MaxSpeed = Mathf.Abs(value);
		}
	}

	[Export]
	public float Speed {
		get {
			return pilot.Speed;
		}
	}


	[Export]
	public bool ControlCamera {get; set;}

	public PlayerShip() {
		pilot = new ShipPilot();
		ControlCamera = true;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		headingCast2D = this.FindNode("RayCast2D", true, true) as RayCast2D;
		frontExhaust = this.FindNode("FrontExhaust", true, true) as Node2D;
		backExhaust = this.FindNode("BackExhaust", true, true) as Node2D;

		pilot.Ready(this, PlayerOneInput.Instance);
	}

	public override void _Process(float delta) {
		pilot.Process(delta);
		frontExhaust.Visible = pilot.Thrusters.Foward > 0;
		backExhaust.Visible = pilot.Thrusters.Backward > 0;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

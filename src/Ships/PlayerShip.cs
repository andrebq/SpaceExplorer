using Godot;
using System;

public class PlayerShip : KinematicBody2D
{
	private RayCast2D headingCast2D;
	private ShipPilot pilot;
	private Node2D frontExhaust;
	private Node2D backExhaust;
	private ColorRect healthBar;

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
		set {
			// ignored
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
		healthBar = this.FindNode("HealthBar", true, true) as ColorRect;

		pilot.Ready(this, PlayerOneInput.Instance);
		healthBar.SetAsToplevel(true);
	}

	public override void _Process(float delta) {
		pilot.Process(delta);
		var newPos = GlobalPosition;
		newPos.y -= 45 + 2.5f;
		newPos.x -= 25;
		healthBar.SetGlobalPosition(newPos, true);
		frontExhaust.Visible = pilot.Thrusters.Foward > 0;
		backExhaust.Visible = pilot.Thrusters.Backward > 0;
	}
}

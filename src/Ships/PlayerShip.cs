using Godot;
using System;

public class PlayerShip : KinematicBody2D
{
	private PackedScene dummyRocketScene;
	private RayCast2D headingCast2D;
	private ShipPilot pilot;
	private Node2D frontExhaust;
	private Node2D backExhaust;
	private ColorRect healthBar;
	
	private Position2D rocketLauncherPosition;

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
		rocketLauncherPosition = this.FindNode("RocketLauncherPosition", true, true) as Position2D;
		dummyRocketScene = (PackedScene)ResourceLoader.Load("res://src/Ammo/DummyRocket.tscn");

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

		if (Input.IsActionJustPressed("FireMainWeapon")) {
			FireMainWeapon();
		}
	}

	public void FireMainWeapon() {
		GD.Print("Fire main weapon");
		DummyRocket dr = (DummyRocket)dummyRocketScene.Instance();
		dr.Heading = new Vector2(Mathf.Sin(Rotation), Mathf.Cos(Rotation));
		dr.GlobalPosition = rocketLauncherPosition.GlobalPosition;
		GetParent().AddChild(dr);
		dr.Fire();
	}
}

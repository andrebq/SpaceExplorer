using Godot;
using System;

public class PlayerShip : Node2D
{
	private KinematicBody2D body2D;
	private RayCast2D headingCast2D;
	private ShipPilot pilot;
	private Node2D rocketExhaust;

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
	public Camera2D Camera {get; set;}

	public PlayerShip() {
		pilot = new ShipPilot();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		body2D = this.FindNode("KinematicBody2D", true, true) as KinematicBody2D;
		headingCast2D = this.FindNode("RayCast2D", true, true) as RayCast2D;
		rocketExhaust = this.FindNode("RocketExhaust", true, true) as Node2D;

		pilot.Ready(body2D, PlayerOneInput.Instance);
	}

	public override void _Process(float delta) {
		pilot.Process(delta);
		rocketExhaust.Visible = pilot.IsThurstersON;
		//body2D.MoveAndCollide(Vector2.Left * body2D.Rotation, true, true, false);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

using Godot;
using System;

public class PlayerShip : KinematicBody2D
{
    [Signal]
    public delegate void Warning(String msg);
    private PackedScene dummyRocketScene;
    private RayCast2D headingCast2D;
    private ShipPilot pilot;
    private Node2D frontExhaust;
    private Node2D backExhaust;
    private ColorRect healthBar;
    private Thruster ForwardThruster;
    private Thruster BackwardThruster;

    private Position2D rocketLauncherPosition;

    [Export]
    public float MaxThrust
    {
        get
        {
            return pilot.MaxThrust;
        }
        set
        {
            pilot.MaxThrust = Mathf.Abs(value);
        }
    }

    [Export]
    public float MaxSpeed
    {
        get
        {
            return pilot.MaxSpeed;
        }
        set
        {
            pilot.MaxSpeed = Mathf.Abs(value);
        }
    }

    public float Speed
    {
        get
        {
            return pilot.Velocity.Length();
        }
    }


    [Export]
    public bool ControlCamera { get; set; }

    public PlayerShip()
    {
        pilot = new ShipPilot();
        ControlCamera = true;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        headingCast2D = this.FindNode("RayCast2D", true, true) as RayCast2D;
        frontExhaust = this.FindNode("FrontExhaust", true, true) as Node2D;
        backExhaust = this.FindNode("BackExhaust", true, true) as Node2D;
        rocketLauncherPosition = this.FindNode("RocketLauncherPosition", true, true) as Position2D;
        dummyRocketScene = (PackedScene)ResourceLoader.Load("res://src/Ammo/DummyRocket.tscn");
        ForwardThruster = this.FindNode(nameof(ForwardThruster)) as Thruster;
        BackwardThruster = this.FindNode(nameof(BackwardThruster)) as Thruster;

        pilot.Ready(this, PlayerOneInput.Instance);
    }

    public override void _PhysicsProcess(float delta) {
        ForwardThruster.Active = PlayerOneInput.Instance.Forward;
        BackwardThruster.Active = PlayerOneInput.Instance.Backward;
    }

    public override void _Process(float delta)
    {
        pilot.Process(delta);
        frontExhaust.Visible = pilot.Thrusters.Foward > 0;
        backExhaust.Visible = pilot.Thrusters.Backward > 0;

        if (Input.IsActionJustPressed("FireMainWeapon"))
        {
            FireMainWeapon(delta);
        }
    }

    public void FireMainWeapon(float delta)
    {
        DummyRocket dr = (DummyRocket)dummyRocketScene.Instance();
        dr.Connect("Hit", this, nameof(hit));

        var rocketHeading = Mathf.Pi / 2 + Rotation;
        dr.Heading = new Vector2(Mathf.Cos(rocketHeading), Mathf.Sin(rocketHeading));
        GetParent().AddChild(dr);
        dr.Fire(Mathf.Abs(Speed), rocketLauncherPosition.GlobalPosition, delta);
    }
    private void hit(Node2D body2D)
    {
        switch (body2D)
        {
            case Meteor meteor:
                meteor.Explode();
                break;
            default:
                // unexpected
                break;
        }
    }
}

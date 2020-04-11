using Godot;
using System;

public class PlayerShip : KinematicBody2D
{
    [Signal]
    public delegate void Warning(String msg);
    private PackedScene dummyRocketScene;
    private RayCast2D headingCast2D;
    private Node2D frontExhaust;
    private Node2D backExhaust;
    private ColorRect healthBar;
    private Thruster ForwardThruster;
    private Thruster BackwardThruster;
    private Steering Steering;
    private Drag SpeedBrake;
    private Pilot Pilot;
    private Position2D rocketLauncherPosition;

    public Vector2 Velocity
    {
        get
        {
            return Pilot.Velocity;
        }
    }

    [Export]
    public bool ControlCamera { get; set; }

    public PlayerShip()
    {
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
        Steering = this.FindNode(nameof(Steering)) as Steering;
        SpeedBrake = this.FindNode(nameof(SpeedBrake)) as Drag;
        Pilot = this.FindNode(nameof(Pilot)) as Pilot;
    }

    public override void _PhysicsProcess(float delta)
    {
        SpeedBrake.Active = PlayerOneInput.Instance.SpeedBrake;
        Pilot.Break = SpeedBrake.Active;
        ForwardThruster.Active = PlayerOneInput.Instance.Forward;
        backExhaust.Visible = ForwardThruster.Active;

        BackwardThruster.Active = PlayerOneInput.Instance.Backward;
        frontExhaust.Visible = BackwardThruster.Active;

        Steering.HorizontalAxis = PlayerOneInput.Instance.HorizontalAxis;
    }

    public override void _Process(float delta)
    {
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
        dr.Fire(Mathf.Abs(Pilot.Velocity.Length()), rocketLauncherPosition.GlobalPosition, delta);
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

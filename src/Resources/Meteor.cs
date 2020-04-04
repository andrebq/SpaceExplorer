using Godot;
using System;

public class Meteor : KinematicBody2D
{
    private readonly RandomNumberGenerator rng;
    private float angularSpeedRad;
    private PackedScene selfScene;
    private float angularSpeedDeg;
    private bool Divisible
    {
        get
        {
            return Scale.x >= 0.90;
        }
    }
    [Export]
    public float AngularSpeed
    {
        get
        {
            return angularSpeedDeg;
        }
        set
        {
            angularSpeedDeg = value;
            angularSpeedRad = Mathf.Deg2Rad(angularSpeedDeg);
        }
    }

    [Export]
    public Vector2 LinearVelocity { get; set; }

    [Export]
    public float Lifetime { get; set; }

    private bool Dead { get { return Lifetime <= 0; } }

    public Meteor()
    {
        AngularSpeed = 180;
        LinearVelocity = Vector2.Zero;
        Lifetime = 1000;
    }

    public override void _Ready()
    {
        selfScene = ResourceLoader.Load("res://src/Resources/Meteor.tscn") as PackedScene;
    }

    public void Explode()
    {
        if (Divisible)
        {
            Subdivide();
        }
        QueueFree();
    }

    private void Subdivide()
    {
        var subScale = Scale / 2;
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        for (var i = 0; i < 4; i++)
        {
            var part = selfScene.Instance() as Meteor;
            part.Scale = subScale;
            var angle = rng.RandfRange(10, 180);
            part.LinearVelocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 1000;
            part.GlobalPosition = GlobalPosition;
            GetParent().AddChild(part);
        }
    }

    public override void _Process(float delta)
    {
        this.MoveAndSlide(LinearVelocity * delta);
        this.Rotate(delta * angularSpeedRad);
        Lifetime -= delta;
        if (Dead)
        {
            QueueFree();
        }
    }
}

using Godot;
using System;

public class DummyRocket : KinematicBody2D
{
    private AnimationPlayer animation;

    [Export]
    public float Lifetime { get; set; }

    [Export]
    public Vector2 Heading { get; set; }

    [Export]
    public float Speed { get; set; }

    public DummyRocket()
    {
        Speed = 1000;
        Lifetime = 3;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animation = FindNode("Animation", true, true) as AnimationPlayer;
    }

    public void Fire()
    {
        animation.Play("Flying");
        Rotation = Heading.Angle() + Mathf.Pi / 2;
    }

    public override void _Process(float delta)
    {
        var collision = MoveAndCollide(Heading * Speed * delta);
        if (collision != null)
        {
            Explode();
        }
        Lifetime -= delta;
        if (Lifetime <= 0)
        {
            Vanish();
        }
    }

    public void Explode()
    {
        Vanish();
    }

    public void Vanish()
    {
        QueueFree();
    }
}

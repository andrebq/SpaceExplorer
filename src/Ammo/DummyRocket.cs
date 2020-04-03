using Godot;
using System;

public class DummyRocket : KinematicBody2D
{
    private AnimationPlayer animation;
    private Particles2D explosion;
    private bool exploded;
    private Node2D missileGraphics;

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
        explosion = FindNode("Explosion") as Particles2D;
        missileGraphics = FindNode("MissileGraphics") as Node2D;
    }

    public void Fire()
    {
        animation.Play("Flying");
        Rotation = Heading.Angle() + Mathf.Pi / 2;
    }

    public override void _Process(float delta)
    {
        if (!exploded) {
            var collision = MoveAndCollide(Heading * Speed * delta);
            if (collision != null)
            {
                Explode();
            }
        }
        Lifetime -= delta;
        if (Lifetime <= 0)
        {
            Vanish();
        }
    }

    public void Explode()
    {
        exploded = true;
        explosion.Emitting = true;
        missileGraphics.Visible = false;
        Lifetime = explosion.Lifetime;
    }

    public void Vanish()
    {
        QueueFree();
    }
}
using Godot;
using System;

public class DummyRocket : KinematicBody2D
{
    private AnimationPlayer animation;

    [Export]
    public float Lifetime {get; set;}

    [Export]
    public Vector2 Heading { get; set; }

    [Export]
    public float Speed { get; set; }

    public DummyRocket() {
        Speed = 100;
        Lifetime = 3;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animation = FindNode("Animation", true, true) as AnimationPlayer;
    }

    public void Fire() {
        animation.Play("Flying");
    }

    public override void _Process(float delta) {
        Rotation = Heading.Angle();
        var collision = MoveAndCollide(Heading.Normalized() * Speed * delta);
        if (collision != null) {
            GD.Print("Collison detected: ", collision);
        }
        Lifetime -= delta;
        if (Lifetime <= 0) {
            GetParent().RemoveChild(this);
        }
    }
}
